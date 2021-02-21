#ifndef _TV_NODE_TREE_VIEW_H_
#define _TV_NODE_TREE_VIEW_H_

#include "UT_ShareBase.h"
#include "UT_SharedPtr.h"
#include "MEM_Override.h"
#include "STD_Containers.h"
#include "STD_Types.h"

#include "TV_FloatDataView.h"
#include "TV_Pos3dDataView.h"
#include "TV_EffectDataView.h"

#include "TR_Types.h"

#include <string>
#include <vector>

class TV_NodeTreeView;

class TR_NodeTree;
class TR_AnimationDataObject;
class TR_DrawingAnimationDataObject;
class TR_DrawingDataObject;
class TR_DataObject;

namespace Math
{
  class Matrix4x4;
}

typedef UT_SharedPtr<TV_NodeTreeView> TV_NodeTreeViewPtr_t;

class TV_NodeTreeView : public UT_ShareBase
{
  MEM_OVERRIDE

  friend class BrotherIterator;
  friend class TV_EffectDataView;

public:

  class BrotherIterator
  {
    MEM_OVERRIDE

  public:

    BrotherIterator();
    BrotherIterator(TV_NodeTreeViewPtr_t nodeTreeView);
    ~BrotherIterator();

    BrotherIterator &operator++();
    BrotherIterator operator++(int);

    bool operator==(const BrotherIterator &it) const;
    bool operator!=(const BrotherIterator &it) const;

    const TV_NodeTreeView &operator*() const;
    TV_NodeTreeView &operator*();

    const TV_NodeTreeView *operator->() const;
    TV_NodeTreeView *operator->();

  private:

    TV_NodeTreeViewPtr_t _nodeTreeView;
  };

  typedef STD_Vector< TV_EffectDataViewPtr_t > EffectCol_t;
  typedef EffectCol_t::const_iterator EffectIterator;

public:

  static TV_NodeTreeViewPtr_t create( const TR_NodeTree *nodeTree );

  virtual ~TV_NodeTreeView();

  bool                   operator==(const TV_NodeTreeView &nodeTreeView) const;
  bool                   operator<(const TV_NodeTreeView &nodeTreeView) const;

  TV_NodeTreeViewPtr_t   parent() const;
  TV_NodeTreeViewPtr_t   find( const STD_String &nodeName ) const;

  BrotherIterator        childBegin() const;
  BrotherIterator        childEnd() const;

  bool                   isValid() const;

  STD_String             name() const;

  float                  totalDuration() const;
  unsigned               depth() const;

  /*!
   * Animated Data
   */
  // @{
  bool                   hasAnimation() const;
  bool                   hasDeformation() const;

  TV_FloatDataViewPtr_t  separateX() const;
  TV_FloatDataViewPtr_t  separateY() const;
  TV_FloatDataViewPtr_t  separateZ() const;

  TV_Pos3dDataViewPtr_t  position() const;

  TV_FloatDataViewPtr_t  scaleX() const;
  TV_FloatDataViewPtr_t  scaleY() const;
  TV_FloatDataViewPtr_t  scaleXY() const;

  TV_FloatDataViewPtr_t  rotationZ() const;

  TV_FloatDataViewPtr_t  skew() const;

  TV_Pos3dDataViewPtr_t  pivot() const;

  TV_FloatDataViewPtr_t  opacity() const;

  void                   position(float frame, float &offsetx, float &offsety, float &offsetz) const;
  void                   scale(float frame, float &scalex, float &scaley) const;
  void                   rotation(float frame, float &rotationz) const;
  void                   skew(float frame, float &skew) const;
  void                   pivot(float frame, float &pivotx, float &pivoty, float &pivotz) const;
  void                   opacity(float frame, float &opacity) const;

  const Math::Matrix4x4 &localMatrix( float frame ) const;
  const Math::Matrix4x4 &modelMatrix( float frame ) const;
  // @}

  /*!
   * Drawing data
   */
  // @{
  bool                   hasDrawingAnimation() const;

  bool                   spriteSheet( STD_String &spriteSheetName ) const;

  unsigned               nSprites() const;

  bool                   sprite( unsigned idx, STD_String &spriteName, float &start, float &duration ) const;
  bool                   sprite( float frame, STD_String &spriteName ) const;
  // @}

  /*!
   * Effect Data
   */
  EffectIterator         effectBegin() const;
  EffectIterator         effectEnd() const;

  /*!
   * Deformation Data
   */
  // @{
  void                   restOffset( float frame, float &offsetx, float &offsety) const;
  void                   restLength( float frame, float &length ) const;
  void                   restRotation( float frame, float &rotation ) const;

  void                   deformOffset( float frame, float &offsetx, float &offsety ) const;
  void                   deformLength( float frame, float &length ) const;
  void                   deformRotation( float frame, float &rotation ) const;

  const Math::Matrix4x4 &restStartMatrix( float frame ) const;
  const Math::Matrix4x4 &restEndMatrix( float frame ) const;

  const Math::Matrix4x4 &deformStartMatrix( float frame ) const;
  const Math::Matrix4x4 &deformEndMatrix( float frame ) const;
  // @}

private:

  void createChannels();
  bool createChannel( TR_Types::CurveChannel_t channelType, TR_Types::DataRef_t dataRef );

private:

  const TR_DrawingAnimationDataObject *drawingAnimationData() const;

  TV_FloatDataViewPtr_t  floatChannel( TR_Types::CurveChannel_t channel ) const;
  TV_Pos3dDataViewPtr_t  pos3dChannel( TR_Types::CurveChannel_t channel ) const;

  const TR_DrawingDataObject *drawingAt( float frame ) const;
  const TR_DrawingDataObject *drawingAt( unsigned idx ) const;

private:
  TV_NodeTreeView( const TR_NodeTree *nodeTree, TR_Types::DataRef_t nodeRef, TV_NodeTreeViewPtr_t parentNode, TV_EffectDataViewPtr_t effectData = 0, TR_Types::EffectId_t matteId = TR_Types::eNoop );

  //  copy is not implemented.
  TV_NodeTreeView( const TV_NodeTreeView & );
  TV_NodeTreeView &operator= ( const TV_NodeTreeView & );

private:

  class Impl;
  Impl *_i;
};

#endif /* _TV_NODE_TREE_VIEW_H_ */
