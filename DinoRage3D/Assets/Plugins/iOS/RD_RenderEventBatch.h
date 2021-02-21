#ifndef _RD_RENDER_BATCH_H_
#define _RD_RENDER_BATCH_H_

namespace Math
{
  class Matrix4x4;
}

class RD_RenderEvent;
class RD_Renderer;

/*!
 *  @class RD_RenderEventBatch
 *  Collection of render events scheduled for rendering.
 */
class RD_RenderEventBatch
{
public:

  RD_RenderEventBatch();
  ~RD_RenderEventBatch();

  void addEvent( int frameCount, int depth, RD_RenderEvent *renderEvent );
  void clear();

  void render( RD_Renderer *renderer, int cullingMask, const Math::Matrix4x4 &projectionMatrix, const Math::Matrix4x4 &viewMatrix ) const;

private:

  void flushEvents();

private:

  //  not implemented.
  RD_RenderEventBatch( const RD_RenderEventBatch& );
  RD_RenderEventBatch &operator= ( const RD_RenderEventBatch& );

private:

  class Impl;
  Impl *_i;
};

#endif /* _RD_RENDER_BATCH_H_ */
