
#ifndef _RD_RENDERER_H_
#define _RD_RENDERER_H_

#include "RD_SpriteSheet.h"
#include "RD_RenderScript.h"

#include "RD_RenderScriptPlain.h"
#include "RD_RenderScriptFx.h"

/*!
 *  @class RD_Renderer
 *  Multi-plaform rendering class.
 */
class RD_Renderer
{
public:

  typedef void* TexturePtr_t;

  struct Rect
  {
    int _offsetx, _offsety;
    int _width, _height;
  };

public:

  RD_Renderer();
  virtual ~RD_Renderer();

  virtual void initialize() = 0;
  virtual void shutdown() = 0;

  virtual void setDevice( void *device, int eventType ) = 0;

  virtual void cleanup() = 0;

  virtual void beginRender() = 0;
  virtual void endRender() = 0;

  virtual void setMatrices( const Math::Matrix4x4 &projectionMatrix, const Math::Matrix4x4 &modelViewMatrix ) = 0;

  virtual void beginSpriteSheet( const RD_SpriteSheetPtr_t &spriteSheet ) = 0;
  virtual void endSpriteSheet() = 0;

  virtual void beginComposition( const RD_RenderScriptPlainPtr_t &renderScript ) = 0;
  virtual void endComposition( const RD_RenderScriptPlainPtr_t &renderScript ) = 0;

  virtual void beginComposition( const RD_RenderScriptFxPtr_t &renderScript ) = 0;
  virtual void endComposition( const RD_RenderScriptFxPtr_t &renderScript ) = 0;

  virtual void beginBatch( const RD_RenderScriptFx::RenderBatch &renderBatch ) = 0;
  virtual void endBatch( const RD_RenderScriptFx::RenderBatch &renderBatch ) = 0;

  virtual void renderScriptToTexture( const RD_RenderScriptPtr_t &pRenderScript, const Rect &rect, TexturePtr_t texture ) = 0;
  virtual void renderScript( const RD_RenderScriptPtr_t &pRenderScript, const Math::Matrix4x4 &projectionMatrix, const Math::Matrix4x4 &modelViewMatrix ) = 0;

  virtual void renderVertices( unsigned short *indices, unsigned indexOffset, unsigned nIndices, unsigned vertexOffset, unsigned nVertices ) = 0;

private:
};



#endif /* _RD_RENDERER_H_ */
