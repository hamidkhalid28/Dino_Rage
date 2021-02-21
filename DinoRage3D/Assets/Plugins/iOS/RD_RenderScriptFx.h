
#ifndef _RD_RENDER_SCRIPT_FX_H_
#define _RD_RENDER_SCRIPT_FX_H_

#include "RD_Composition.h"
#include "RD_SpriteSheet.h"
#include "RD_RenderScript.h"

#include "STD_Types.h"
#include "STD_Containers.h"

class RD_Renderer;
class RD_RenderScriptFx;

typedef UT_SharedPtr<RD_RenderScriptFx> RD_RenderScriptFxPtr_t;
typedef UT_SharedWeakPtr<RD_RenderScriptFx> RD_RenderScriptFxWeakPtr_t;

// #ifndef MAX_EFFECTS
// #define MAX_EFFECTS 15
// #endif

#define MAX_BONES_GPU    32
#define MAX_BONES_GPUf   32.0f

/*!
 *  @class RD_RenderScriptFx
 *  Rendering data for a single clip.
 */
class RD_RenderScriptFx : public RD_RenderScript
{
public:

  /*!
   *  @struct VertexData
   *  Vertex rendering parameters.
   */
  struct VertexData
  {
    float _x, _y, _z;
    unsigned int _color;

    float _u0, _v0;

    float _fxParams0[4];
    float _fxViewport0[4];

    float _boneParams[4];
  };

  typedef STD_Vector<VertexData> VertexDataCol_t;

  typedef unsigned short Index_t;
  typedef Index_t IndexPtr_t;

  typedef STD_Vector<Index_t> IndexDataCol_t;

  /*!
   *  @struct BoneMatrix
   *  Raw matrix data.
   */
  struct BoneMatrix
  {
    float _data[16];
  };

  typedef STD_Vector< BoneMatrix > BoneMatrixCol_t;

  /*!
   *  @struct RenderBatch
   *  Batch rendering parameters.
   */
  struct RenderBatch
  {
    RD_SpriteSheetPtr_t _spriteSheet;
    unsigned            _indexOffset;
    unsigned            _nIndices;
    unsigned            _vertexOffset;
    unsigned            _nVertices;

    BoneMatrixCol_t     _uniformBoneMatrices;
  };

  typedef STD_Vector< RenderBatch > RenderBatchCol_t;

public:

  RD_RenderScriptFx();
  virtual ~RD_RenderScriptFx();

  virtual void update( const RD_ClipDataPtr_t &pClipData, const STD_String &projectFolder, const STD_String &sheetResolution, float frame, unsigned int color );
  virtual void render( RD_Renderer *renderer, const Math::Matrix4x4 &projectionMatrix, const Math::Matrix4x4 &modelViewMatrix );

  virtual void calculateBoundingBox( BoundingBox &box );
  virtual void calculateConvexHull( BoundingVertex* &convexHullArray, int &convexHullSize );
  virtual void calculatePolygons( BoundingVertex* &polygonArray, int &polygonSize, int* &subPolygonArray, int &subPolygonSize );

  virtual bool supportsFeature( Feature feature ) const;

  static Feature supportedFeatures();

public:

  bool isDirty( float frame ) const;
  bool isRenderDirty() const;

  const VertexData *vertices() const;
  size_t verticesCount() const;

  const Index_t *indices() const;
  size_t indicesCount() const;

protected:

  void lock();
  void unlock();

  void cleanup();

  void updateBatch( const RD_Composition *composition, unsigned int color, RenderBatch &renderBatch );

private:

  class Impl;
  Impl *_i;

};

#endif /* _RD_RENDER_SCRIPT_FX_H_ */
