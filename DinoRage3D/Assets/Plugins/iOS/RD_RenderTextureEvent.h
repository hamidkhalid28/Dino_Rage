
#ifndef _RD_RENDER_TEXTURE_EVENT_H_
#define _RD_RENDER_TEXTURE_EVENT_H_

#include "RD_RenderObjectManager.h"
#include "RD_Renderer.h"

#include "UT_ShareBaseSafe.h"
#include "UT_SharedPtr.h"

class RD_RenderTextureEvent;
typedef UT_SharedPtr< RD_RenderTextureEvent > RD_RenderTextureEventPtr_t;

//  Thread-safe collection.
typedef RD_RenderObjectManager<int, RD_RenderTextureEvent> RD_RenderTextureEventManager;

class RD_RenderTextureEvent : public UT_ShareBaseSafe
{
public:

  typedef void* TexturePtr_t;

public:

  RD_RenderTextureEvent();
  RD_RenderTextureEvent( int scriptId, int offsetx, int offfsety, int width, int height, TexturePtr_t texture );

  void render( RD_Renderer *renderer );

private:

  int               _scriptId;
  RD_Renderer::Rect _rect;
  TexturePtr_t      _texture;

};

#endif /* _RD_RENDER_TEXTURE_EVENT_H_ */
