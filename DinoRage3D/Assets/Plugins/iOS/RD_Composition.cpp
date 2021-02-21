#include "RD_Composition.h"

namespace
{
  /* comparators */
  struct CompositionNodeComparator
  {
    bool operator()( const RD_Composition::CompositionNode *node1, const RD_Composition::CompositionNode *node2 )
    {
      return ( (*node1) < (*node2) );
    }
  };

  struct EffectNodeComparator
  {
    EffectNodeComparator( const TV_EffectDataViewPtr_t &effectDataView, const TV_NodeTreeViewPtr_t &matteNodeTreeView = 0 ) :
      _effectDataView(effectDataView),
      _matteNodeTreeView(matteNodeTreeView)
    {
    }

    bool operator()( const RD_Composition::CutterEffect *effect )
    {
      if ( (*_effectDataView) == (*effect->_effectDataView) )
      {
        if ( !effect->_nodeTreeView.isValid() && !_matteNodeTreeView.isValid() )
          return true;

        return ((*_matteNodeTreeView) == (*effect->_nodeTreeView));
      }

      return false;
    }

    bool operator()( const RD_Composition::BoneEffect *effect )
    {
      if ( (*_effectDataView) == (*effect->_effectDataView) )
      {
        if ( !effect->_nodeTreeView.isValid() && !_matteNodeTreeView.isValid() )
          return true;

        return ((*_matteNodeTreeView) == (*effect->_nodeTreeView));
      }

      return false;
    }

    TV_EffectDataViewPtr_t _effectDataView;
    TV_NodeTreeViewPtr_t   _matteNodeTreeView;
  };

  /* builders */
  void populateBoneParams( float frame, RD_Composition::BoneEffect *bone )
  {
    const Math::Matrix4x4 &restStartMatrix = bone->_nodeTreeView->restStartMatrix(frame);
    const Math::Matrix4x4 &restEndMatrix = bone->_nodeTreeView->restEndMatrix(frame);

    bone->_restStart = restStartMatrix * Math::Point3d(0,0,0);
    bone->_restEnd = restEndMatrix * Math::Point3d(0,0,0);

    const Math::Matrix4x4 &deformStartMatrix = bone->_nodeTreeView->deformStartMatrix(frame);
    const Math::Matrix4x4 &deformEndMatrix = bone->_nodeTreeView->deformEndMatrix(frame);

    bone->_deformStart = deformStartMatrix * Math::Point3d(0,0,0);
    bone->_deformEnd = deformEndMatrix * Math::Point3d(0,0,0);

    Math::Matrix4x4 restStartMatrixInverse = restStartMatrix;
    Math::Matrix4x4 restEndMatrixInverse = restEndMatrix;

    restStartMatrixInverse.inverse();
    restEndMatrixInverse.inverse();

    Math::Matrix4x4 deformScaleMatrix;

    float restLength, deformLength;
    bone->_nodeTreeView->restLength(frame, restLength);
    bone->_nodeTreeView->deformLength(frame, deformLength);

    if ( restLength != 0.0f )
    {
      deformScaleMatrix.scale( deformLength / restLength, 1.0 );
    }

    bone->_preMatrix = deformStartMatrix * restStartMatrixInverse;
    bone->_matrix = deformStartMatrix * deformScaleMatrix * restStartMatrixInverse;
    bone->_postMatrix = deformEndMatrix * restEndMatrixInverse;
  }

  void populateCutterParams( float frame, const RD_SpriteSheetPtr_t &spriteSheet, const Math::Matrix4x4 *parentMatrix, RD_Composition::CutterEffect *cutter )
  {
    if ( cutter->_nodeTreeView->sprite( frame, cutter->_spriteName ) )
    {
      const RD_SpriteSheet::SpriteData *spriteData = spriteSheet->sprite( cutter->_spriteName );
      if ( spriteData )
      {
        const Math::Matrix4x4 *modelMatrix = &cutter->_nodeTreeView->modelMatrix(frame);

        //  Model matrix is parentMatrix * modelMatrix * spriteMatrix. 
        cutter->_matrix = parentMatrix ? ( (*parentMatrix) * (*modelMatrix) ) : (*modelMatrix);
        cutter->_matrix.multiply(spriteData->_matrix);
        cutter->_matrix.inverse();
      }
    }
  }

  void buildCutterCompositionIter( float frame, const RD_SpriteSheetPtr_t &spriteSheet, const TV_EffectDataViewPtr_t &effectDataView,
                                   const TV_NodeTreeViewPtr_t &nodeTreeView, const Math::Matrix4x4 *parentMatrix,
                                   RD_Composition::CompositionNode *node, RD_Composition::CutterEffectCol_t &cutters )
  {
    if ( nodeTreeView->nSprites() > 0 )
    {
      RD_Composition::CutterEffectCol_t::const_iterator iCutter = STD_FindIf( cutters.begin(), cutters.end(), EffectNodeComparator( effectDataView, nodeTreeView ) );
      if ( iCutter == cutters.end() )
      {
        RD_Composition::CutterEffect *cutter = new RD_Composition::CutterEffect;
        cutter->_effectDataView = effectDataView;
        cutter->_nodeTreeView = nodeTreeView;
        cutter->_effectIdx = (unsigned int)cutters.size();

        populateCutterParams( frame, spriteSheet, parentMatrix, cutter );

        //  Append cutter to global collection.
        cutters.push_back( cutter );

        //  Append cutter to composition node local collection
        node->_cutters.push_back( cutter );
      }
      else
      {
        //  Append cutter to composition node local collection
        node->_cutters.push_back( *iCutter );
      }
    }

    for ( TV_NodeTreeView::BrotherIterator i = nodeTreeView->childBegin(), iEnd = nodeTreeView->childEnd() ; i!=iEnd ; ++i )
    {
      buildCutterCompositionIter( frame, spriteSheet, effectDataView, &*i, parentMatrix, node, cutters );
    }
  }

  void buildDeformationCompositionIter( float frame, const TV_EffectDataViewPtr_t &effectDataView, const TV_NodeTreeViewPtr_t &nodeTreeView,
                                        const Math::Matrix4x4 *parentMatrix, RD_Composition::CompositionNode *node, RD_Composition::BoneEffect *parentBone,
                                        RD_Composition::BoneEffectCol_t &bones )
  {
    if ( nodeTreeView->hasDeformation() )
    {
      RD_Composition::BoneEffectCol_t::const_iterator iBone = STD_FindIf( bones.begin(), bones.end(), EffectNodeComparator( effectDataView, nodeTreeView ) );
      if ( iBone == bones.end() )
      {
        RD_Composition::BoneEffect *bone = new RD_Composition::BoneEffect;
        bone->_effectDataView = effectDataView;
        bone->_nodeTreeView = nodeTreeView;
        bone->_parentBone = parentBone;
        bone->_effectIdx = (unsigned int)bones.size();

        populateBoneParams( frame, bone );

        if ( parentBone )
          parentBone->_childBones.push_back( bone );

        parentBone = bone;

        //  Append bone to global collection.
        bones.push_back( bone );

        //  Append bone to composition node local collection
        node->_bones.push_back( bone );

      }
      else
      {
        //  Append bone to composition node local collection
        node->_bones.push_back( *iBone );

        parentBone = *iBone;
      }
    }

    for ( TV_NodeTreeView::BrotherIterator i = nodeTreeView->childBegin(), iEnd = nodeTreeView->childEnd() ; i!=iEnd ; ++i )
    {
      buildDeformationCompositionIter( frame, effectDataView, &*i, parentMatrix, node, parentBone, bones );
    }
  }

  void buildCompositionIter( float frame, const RD_SpriteSheetPtr_t &spriteSheet, const TV_NodeTreeViewPtr_t &nodeTreeView, const Math::Matrix4x4 *parentMatrix,
                             unsigned &orderIdx, RD_Composition::CutterEffectCol_t &cutters, RD_Composition::BoneEffectCol_t &bones, RD_Composition::CompositionNodeCol_t &nodes )
  {
    if ( nodeTreeView->nSprites() > 0 )
    {
      RD_Composition::CompositionNode *node = new RD_Composition::CompositionNode;
      nodes.push_back( node );

      node->_nodeTreeView = nodeTreeView;

      //  order idx is set once during build and will be used to sort composition
      //  when two layers are at the same z offset.
      node->_orderIdx = ++orderIdx;

      if ( nodeTreeView->sprite( frame, node->_spriteName ) )
      {
        const RD_SpriteSheet::SpriteData *spriteData = spriteSheet->sprite( node->_spriteName );
        if ( spriteData )
        {
          const Math::Matrix4x4 *modelMatrix = &node->_nodeTreeView->modelMatrix(frame);

          //  Model matrix is parentMatrix * modelMatrix * spriteMatrix. 
          node->_matrix = parentMatrix ? ( (*parentMatrix) * (*modelMatrix) ) : (*modelMatrix);
          node->_matrix.multiply(spriteData->_matrix);

          //  Offset z is used sort composition in conjunction with orderIdx.
          node->_offsetZ = node->_matrix.origin().z();

          //  Update layer opacity.
          node->_nodeTreeView->opacity( frame, node->_opacity );
        }
      }

      // Build potential effects for composition.  
      for ( TV_NodeTreeView::EffectIterator i = nodeTreeView->effectBegin(), iEnd = nodeTreeView->effectEnd() ; i!=iEnd ; ++i )
      {
        TV_EffectDataViewPtr_t effectDataView = *i;

        if ( effectDataView->effectId() & TR_Types::eAllCutters )
        {
          //  Effect with matte.  Create one entry for every valid rendered sprite.
          if ( effectDataView->matte().isValid() )
          {
            buildCutterCompositionIter( frame, spriteSheet, effectDataView, effectDataView->matte(), parentMatrix, node, cutters );
          }
          /*
          //  Effect without matte.  Create effect directly.
          else
          {
            RD_Composition::CutterEffectCol_t::const_iterator iEffect = STD_FindIf( effects.begin(), effects.end(), EffectNodeComparator( effectDataView ) );
            if ( iEffect == effects.end() )
            {
              RD_Composition::CutterEffect *effect = new RD_Composition::CutterEffect;
              effect->_effectDataView = effectDataView;
              effect->_effectIdx = effects.size();

              //  Append effect to global collection.
              effects.push_back( effect );

              //  Append effect to composition node local collection
              node->_effects.push_back( effect );
            }
            else
            {
              //  Append effect to composition node local collection
              node->_effects.push_back( *iEffect );
            }
          }
          */
        }
        else if ( effectDataView->effectId() == TR_Types::eDeformation )
        {
          //  Effect with matte.  Create one entry for every valid rendered sprite.
          if ( effectDataView->matte().isValid() )
          {
            buildDeformationCompositionIter( frame, effectDataView, effectDataView->matte(), parentMatrix, node, 0, bones );
          }
        }
      }
    }

    for ( TV_NodeTreeView::BrotherIterator i = nodeTreeView->childBegin(), iEnd = nodeTreeView->childEnd() ; i!=iEnd ; ++i )
    {
      buildCompositionIter( frame, spriteSheet, &*i, parentMatrix, orderIdx, cutters, bones, nodes );
    }
  }
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#if 0
#pragma mark - RD_Composition::Impl
#endif

class RD_Composition::Impl
{
  friend class RD_Composition;

  typedef RD_Composition::CompositionNodeCol_t CompositionNodeCol_t;
  typedef RD_Composition::CutterEffectCol_t CutterEffectCol_t;
  typedef RD_Composition::BoneEffectCol_t BoneEffectCol_t;

public:

  Impl()
  {
  }

  ~Impl()
  {
  }

private:

  RD_SpriteSheetPtr_t     _spriteSheet;
  TV_NodeTreeViewPtr_t    _nodeTreeView;
  TV_NodeTreeViewPtr_t    _parentNodeTreeView;

  CompositionNodeCol_t    _nodes;
  CutterEffectCol_t       _cutters;
  BoneEffectCol_t         _bones;

};

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#if 0
#pragma mark - RD_Composition
#endif

RD_Composition::RD_Composition( const RD_SpriteSheetPtr_t &spriteSheet, const TV_NodeTreeViewPtr_t &nodeTreeView, const TV_NodeTreeViewPtr_t &parentNodeTreeView )
{
  _i = new Impl;

  _i->_spriteSheet = spriteSheet;
  _i->_nodeTreeView = nodeTreeView;
  _i->_parentNodeTreeView = parentNodeTreeView;
}

RD_Composition::~RD_Composition()
{
  cleanup();
  delete _i;
}

void RD_Composition::build( float frame )
{
  cleanup();

  const Math::Matrix4x4 *parentMatrix = 0;
  if ( _i->_parentNodeTreeView.isValid() )
  {
    parentMatrix = &(_i->_parentNodeTreeView->modelMatrix(frame));
  }

  //  build composition from node tree view.
  unsigned orderIdx = 0;
  buildCompositionIter( frame, _i->_spriteSheet, _i->_nodeTreeView, parentMatrix, orderIdx, _i->_cutters, _i->_bones, _i->_nodes );

  //  sort composition.
  STD_Sort( _i->_nodes.begin(), _i->_nodes.end(), CompositionNodeComparator() );
}

void RD_Composition::update( float frame )
{
  //  Retrieve parent model matrix from referenced node tree view.
  const Math::Matrix4x4 *parentMatrix = 0;
  if ( _i->_parentNodeTreeView.isValid() )
  {
    parentMatrix = &(_i->_parentNodeTreeView->modelMatrix(frame));
  }

  //  Update every composition node with new rendered sprite matrix and ordering.
  for ( CompositionNodeCol_t::iterator i = _i->_nodes.begin(), iEnd = _i->_nodes.end() ; i!=iEnd ; ++i )
  {
    CompositionNode *node = *i;
    if ( node->_nodeTreeView->sprite( frame, node->_spriteName ) )
    {
      const RD_SpriteSheet::SpriteData *spriteData = _i->_spriteSheet->sprite( node->_spriteName );
      if ( spriteData )
      {
        const Math::Matrix4x4 *modelMatrix = &node->_nodeTreeView->modelMatrix(frame);

        //  Model matrix is parentMatrix * modelMatrix * spriteMatrix. 
        node->_matrix = parentMatrix ? ( (*parentMatrix) * (*modelMatrix) ) : (*modelMatrix);
        node->_matrix.multiply(spriteData->_matrix);

        //  Offset z is used sort composition in conjunction with orderIdx.
        node->_offsetZ = node->_matrix.origin().z();

        //  Update layer opacity.
        node->_nodeTreeView->opacity( frame, node->_opacity );
      }
    }
    else
    {
      //  Clear name, composition data entry will be ignored at rendering.
      node->_spriteName.clear();
    }
  }

  //  Sort composition.
  STD_Sort( _i->_nodes.begin(), _i->_nodes.end(),  CompositionNodeComparator()  );

  //  Update every cutters with new matte coordinates if available.
  for ( CutterEffectCol_t::iterator i = _i->_cutters.begin(), iEnd = _i->_cutters.end() ; i!=iEnd ; ++i )
  {
    CutterEffect *cutter = *i;

    //  No matte in current effect.
    if ( !cutter->_nodeTreeView.isValid() )
      continue;

    //  Update current sprite used in cutter.
    populateCutterParams( frame, _i->_spriteSheet, parentMatrix, cutter );
  }

  //  Update every bones with new coordinates.
  for ( BoneEffectCol_t::iterator i = _i->_bones.begin(), iEnd = _i->_bones.end() ; i!=iEnd ; ++i )
  {
    BoneEffect *bone = *i;

    //  No matte in current effect.
    if ( !bone->_nodeTreeView.isValid() )
      continue;

    //  Update current bone in deformation.
    populateBoneParams(frame, bone);
  }
}

RD_Composition::CompositionNodeCol_t::const_iterator RD_Composition::nodeBegin() const
{
  return _i->_nodes.begin();
}

RD_Composition::CompositionNodeCol_t::const_iterator RD_Composition::nodeEnd() const
{
  return _i->_nodes.end();
}

unsigned RD_Composition::nodeCount() const
{
  return (unsigned)_i->_nodes.size();
}

RD_Composition::CutterEffectCol_t::const_iterator RD_Composition::cutterBegin() const
{
  return _i->_cutters.begin();
}

RD_Composition::CutterEffectCol_t::const_iterator RD_Composition::cutterEnd() const
{
  return _i->_cutters.end();
}

unsigned RD_Composition::cutterCount() const
{
  return (unsigned)_i->_cutters.size();
}

RD_Composition::BoneEffectCol_t::const_iterator RD_Composition::boneBegin() const
{
  return _i->_bones.begin();
}

RD_Composition::BoneEffectCol_t::const_iterator RD_Composition::boneEnd() const
{
  return _i->_bones.end();
}

unsigned RD_Composition::boneCount() const
{
  return (unsigned)_i->_bones.size();
}

const RD_SpriteSheetPtr_t &RD_Composition::spriteSheet() const
{
  return _i->_spriteSheet;
}

void RD_Composition::cleanup()
{
  //  Cleanup composition nodes.
  for( CompositionNodeCol_t::const_iterator i = _i->_nodes.begin(), iEnd = _i->_nodes.end() ; i!=iEnd ; ++i )
  {
    delete *i;
  }
  _i->_nodes.clear();

  //  Cleanup cutter effects.
  for( CutterEffectCol_t::const_iterator i = _i->_cutters.begin(), iEnd = _i->_cutters.end() ; i!=iEnd ; ++i )
  {
    delete *i;
  }
  _i->_cutters.clear();

  //  Cleanup bone effects.
  for( BoneEffectCol_t::const_iterator i = _i->_bones.begin(), iEnd = _i->_bones.end() ; i!=iEnd ; ++i )
  {
    delete *i;
  }
  _i->_bones.clear();

}
