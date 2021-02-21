
#ifndef _RD_RENDER_SCRIPT_H_
#define _RD_RENDER_SCRIPT_H_

#include "RD_Composition.h"
#include "RD_ClipData.h"
#include "RD_RenderObjectManager.h"

#include "UT_SharedWeakPtr.h"
#include "STD_Types.h"

class RD_Renderer;
class RD_RenderScript;

typedef UT_SharedPtr<RD_RenderScript> RD_RenderScriptPtr_t;
typedef UT_SharedWeakPtr<RD_RenderScript> RD_RenderScriptWeakPtr_t;

typedef RD_RenderObjectManager<int, RD_RenderScript> RD_RenderScriptManager;

/*!
 *  @class RD_RenderScript
 *  Rendering data for a single clip.
 */
class RD_RenderScript : public UT_SharedWeakBase
{
public:

  /*!
   *  @struct BoundingBox
   *  Basic bounding box container.
   */
  struct BoundingBox
  {
    float _x1, _y1;
    float _x2, _y2;
  };

  /*!
   *  @struct BoundingQuad
   *  Basic quad data structure.
   */
  struct BoundingQuad
  {
    float _x1, _y1;
    float _x2, _y2;
    float _x3, _y3;
    float _x4, _y4;
  };

  /*!
   *  @struct BoundingVertex
   *  Basic vertex for polygon data structure.
   */
  struct BoundingVertex
  {
    BoundingVertex( float x, float y ) :
      _x(x),
      _y(y)
    {
    }

    BoundingVertex()
    {
    }

    BoundingVertex( const BoundingVertex &v ) :
      _x(v._x),
      _y(v._y)
    {
    }

    float _x, _y;
  };

  /*!
   *  @enum Feature
   *  Features implemented by current script.
   */
  enum Feature
  {
    eNullFeature                = 0,
    ePlainFeature               = 1<<0,
    eCutterFeature              = 1<<1,
    eDeformationFeature         = 1<<2
  };

public:

  RD_RenderScript();
  virtual ~RD_RenderScript();

  virtual void update( const RD_ClipDataPtr_t &pClipData, const STD_String &projectFolder, const STD_String &sheetResolution, float frame, unsigned int color ) =0;
  virtual void render( RD_Renderer *renderer, const Math::Matrix4x4 &projectionMatrix, const Math::Matrix4x4 &modelViewMatrix ) =0;

  virtual void calculateBoundingBox( BoundingBox &box ) =0;

  virtual void calculateConvexHull( BoundingVertex* &convexHullArray, int &convexHullSize ) =0;
  void         deallocateConvexHull( BoundingVertex* convexHullArray );

  virtual void calculatePolygons( BoundingVertex* &polygonArray, int &polygonSize, int* &subPolygonArray, int &subPolygonSize ) =0;
  void         deallocatePolygons( BoundingVertex* polygonArray, int *subPolygonArray );

  virtual bool supportsFeature( Feature feature ) const = 0;

};

#endif /* _RD_RENDER_SCRIPT_H_ */
