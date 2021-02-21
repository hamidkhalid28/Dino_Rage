
#include "RD_RendererGLES2.h"

#if defined(SUPPORT_RENDERER_GLES2) || defined(SUPPORT_RENDERER_GL)

#include "RD_ShaderProgramGLES2.h"

#include "STD_Containers.h"

#include <stdio.h>

//#define ENABLE_GL_DEBUG
#define ENABLE_VERTEX_BUFFER_OBJECT

#if defined(TARGET_MAC) || defined(TARGET_IOS)
#define ENABLE_VERTEX_ARRAY_OBJECT
#endif

#if defined(SUPPORT_RENDERER_GLES2)
#if defined(TARGET_IOS)
  #include <OpenGLES/ES2/gl.h>
  #include <OpenGLES/ES2/glext.h>
#else //TARGET_ANDROID
  #include <GLES2/gl2.h>
  #include <GLES2/gl2ext.h>
#endif

  #define p_glDeleteVertexArrays    glDeleteVertexArraysOES
  #define p_glGenVertexArrays       glGenVertexArraysOES
  #define p_glBindVertexArray       glBindVertexArrayOES
#elif defined(SUPPORT_RENDERER_GL)
  #if defined(TARGET_MAC)
    #include <OpenGL/gl.h>
    #define p_glDeleteVertexArrays    glDeleteVertexArraysAPPLE
    #define p_glGenVertexArrays       glGenVertexArraysAPPLE
    #define p_glBindVertexArray       glBindVertexArrayAPPLE
  #else
    #define ENABLE_GLEW
    #include <GL/glew.h>
  #endif
#endif

#ifdef ENABLE_GL_DEBUG
  namespace
  {
    void CheckGLError(const char* file, int line)
    {
      GLenum e = glGetError();
      if( e )
        fprintf ( stderr, "OpenGL error 0x%04X in %s:%i\n", e, file, line);
    }
  }

  #define GLAssert()	do { CheckGLError (__FILE__, __LINE__); } while(0)
  #define GL_CHK(expr)	do { {expr;} GLAssert(); } while(0)
#else
  #define GLAssert()	do { } while(0)
  #define GL_CHK(expr)	do { expr; } while(0)
#endif

#ifdef TARGET_ANDROID
#include <android/log.h>

#define  LOGI(...)  __android_log_print(ANDROID_LOG_INFO,LOG_TAG,__VA_ARGS__)
#define  LOGE(...)  __android_log_print(ANDROID_LOG_ERROR,ERROR_TAG,__VA_ARGS__)
#else
#define  LOGI(...)  fprintf(stderr, "[%s] ", LOG_TAG );   fprintf(stderr, __VA_ARGS__)
#define  LOGE(...)  fprintf(stderr, "[%s] ", ERROR_TAG ); fprintf(stderr, __VA_ARGS__)
#endif

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#if 0
#pragma mark - RD_RendererGLES2::Impl
#endif

class RD_RendererGLES2::Impl
{
  friend class RD_RendererGLES2;

public:
  Impl() :
    _activeProgram(0)
  {
  }

  ~Impl()
  {
  }

private:

  typedef STD_Map< RD_SpriteSheetWeakPtr_t, GLuint > TextureIdCol_t;
  TextureIdCol_t _textureIds;

#ifdef ENABLE_VERTEX_BUFFER_OBJECT
  struct BufferData
  {
#ifdef ENABLE_VERTEX_ARRAY_OBJECT
    GLuint _vertexArrayId;
#endif // ENABLE_VERTEX_ARRAY_OBJECT

    GLuint _vertexBufferId;
    GLuint _indexBufferId;
  };

  typedef STD_Map< RD_RenderScriptWeakPtr_t, BufferData > BufferIdCol_t;
  BufferIdCol_t _bufferIds;
#endif // ENABLE_VERTEX_BUFFER_OBJECT

  struct FboData
  {
    GLuint _textureId;
    bool   _owned;

    int    _width;
    int    _height;

    GLuint _fboId;
    //GLuint _rboId;
  };

  typedef STD_Map< RD_RenderScriptWeakPtr_t, FboData > FboIdCol_t;
  FboIdCol_t _fboIds;

  enum ShaderMode_t
  {
    eNullShader,
    ePlainShader,
    eFxShader
  };

  typedef STD_Map< ShaderMode_t, RD_ShaderProgramGLES2* > ShaderProgramCol_t;
  ShaderProgramCol_t _shaders;

  RD_ShaderProgramGLES2 *_activeProgram;
};


////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#if 0
#pragma mark - RD_RendererGLES2
#endif

RD_RendererGLES2::RD_RendererGLES2()
{
  _i = new Impl;
}

RD_RendererGLES2::~RD_RendererGLES2()
{
  delete _i;
}

void RD_RendererGLES2::initialize()
{
#if defined(ENABLE_GLEW)
  glewInit();
#endif // ENABLE_GLEW
}

void RD_RendererGLES2::shutdown()
{
  //  Nothing to do.
}

void RD_RendererGLES2::setDevice( void * /*device*/, int /*eventType*/ )
{
  //  Nothing to do.
}

void RD_RendererGLES2::cleanup()
{
  //  Clear unused texture ids.
  for ( Impl::TextureIdCol_t::iterator i = _i->_textureIds.begin(), iEnd = _i->_textureIds.end() ; i!=iEnd ; )
  {
    if ( !i->first.isValid() )
    {
      glDeleteTextures( 1, &i->second );
      _i->_textureIds.erase( i++ );
    }
    else
    {
      ++i;
    }
  }

  bool deleteProgram = _i->_textureIds.empty();

#ifdef ENABLE_VERTEX_BUFFER_OBJECT
  //  Clear unused buffer ids.
  for ( Impl::BufferIdCol_t::iterator i = _i->_bufferIds.begin(), iEnd = _i->_bufferIds.end() ; i!=iEnd ; )
  {
    if ( !i->first.isValid() )
    {
      glDeleteBuffers( 1, &i->second._vertexBufferId );
      glDeleteBuffers( 1, &i->second._indexBufferId );

#ifdef ENABLE_VERTEX_ARRAY_OBJECT
      p_glDeleteVertexArrays(1, &i->second._vertexArrayId );
#endif

      _i->_bufferIds.erase( i++ );
    }
    else
    {
      ++i;
    }
  }

  deleteProgram = deleteProgram && _i->_bufferIds.empty();
#endif

  for ( Impl::FboIdCol_t::iterator i = _i->_fboIds.begin(), iEnd = _i->_fboIds.end() ; i!=iEnd ; )
  {
    if ( !i->first.isValid() )
    {
      //glDeleteRenderBuffers(1, &i->second._rboId);
      glDeleteFramebuffers(1, &i->second._fboId);

      if ( i->second._owned )
        glDeleteTextures( 1, &i->second._textureId );

      _i->_fboIds.erase( i++ );
    }
    else
    {
      ++i;
    }
  }

  deleteProgram = deleteProgram && _i->_fboIds.empty();

  //  If there is no more data mapped in renderer, free shader as well.
  if ( deleteProgram )
  {
    for ( Impl::ShaderProgramCol_t::iterator i = _i->_shaders.begin(), iEnd = _i->_shaders.end() ; i!=iEnd ; )
    {
      i->second->unload();
      delete i->second;

      _i->_shaders.erase(i++);
    }
  }
}

void RD_RendererGLES2::beginRender()
{
  //  Unbind buffers.
  glBindBuffer(GL_ARRAY_BUFFER, 0);
  glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);

  glDisable(GL_CULL_FACE);

  //  Make sure active texture is 1st one.
  glActiveTexture(GL_TEXTURE0);

  //  Using premultiplied textures.
  glEnable(GL_BLEND);
  glBlendFunc(GL_ONE, GL_ONE_MINUS_SRC_ALPHA);

  glEnable(GL_DEPTH_TEST);
  glDepthFunc(GL_LEQUAL);

  //glPolygonMode(GL_FRONT_AND_BACK, GL_LINE);

  //  Clear unused data.
  cleanup();

  //  None of our shaders are active at this point.
  _i->_activeProgram = 0;
}

void RD_RendererGLES2::endRender()
{
  //glPolygonMode(GL_FRONT_AND_BACK, GL_FILL);
}

void RD_RendererGLES2::setMatrices( const Math::Matrix4x4 &projectionMatrix, const Math::Matrix4x4 &modelViewMatrix )
{
  if ( _i->_activeProgram == 0 )
    return;

  Math::Matrix4x4 mvpMatrix( projectionMatrix * modelViewMatrix );

  float array[16];
  mvpMatrix.getValues(array);

  _i->_activeProgram->setMatrix( array );
}

void RD_RendererGLES2::renderScriptToTexture( const RD_RenderScriptPtr_t &pRenderScript, const Rect &rect, TexturePtr_t texture )
{
  RD_RenderScriptWeakPtr_t pWeakRenderScript(pRenderScript); // for comparison.
  Impl::FboIdCol_t::iterator iFboId = _i->_fboIds.find( pWeakRenderScript );

  GLuint textureId = (GLuint)(size_t)(texture);

  bool createFbo = ( iFboId == _i->_fboIds.end() );
  if ( !createFbo )
  {
    if ( iFboId->second._textureId != textureId )
    {
      //glDeleteRenderBuffers(1, &FboId->second._rboId);
      glDeleteFramebuffers(1, &iFboId->second._fboId);

      if ( iFboId->second._owned )
        glDeleteTextures( 1, &iFboId->second._textureId );

      _i->_fboIds.erase(iFboId);

      createFbo = true;
    }
  }

  //  Backup initial viewport values.
  int viewport[4];
  glGetIntegerv( GL_VIEWPORT, viewport );

  //  Backup initial frame buffer object id.
  GLint prevFboId;
  glGetIntegerv(GL_FRAMEBUFFER_BINDING, &prevFboId);

  if ( createFbo )
  {
    Impl::FboData data;
    data._textureId = textureId;
    data._owned = false; /* texture comes from external source */
    data._width = rect._width;
    data._height = rect._height;

    glBindTexture (GL_TEXTURE_2D, textureId);

    glGenFramebuffers(1, &data._fboId);
    glBindFramebuffer(GL_FRAMEBUFFER, data._fboId);

    glFramebufferTexture2D(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0, GL_TEXTURE_2D, data._textureId, 0);

    //glGenRenderbuffers(1, &data._rboId);
    //glBindRenderbuffer(GL_RENDERBUFFER, data._rboId);

    //glRenderbufferStorage(GL_RENDERBUFFER, GL_DEPTH_COMPONENT16, data._width, data._height);
    //glFramebufferRenderbuffer(GL_FRAMEBUFFER, GL_DEPTH_ATTACHMENT, GL_RENDERBUFFER, data._rboId);

    iFboId = _i->_fboIds.insert( STD_MakePair( pWeakRenderScript, data ) ).first;
  }
  else
  {
    glBindFramebuffer(GL_FRAMEBUFFER, iFboId->second._fboId);
    //glBindRenderbuffer(GL_RENDERBUFFER, iFboId->second._rboId);
  }

  glClearColor( 0.0f, 0.0f, 0.0f, 0.0f );
  glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
  glColorMask(GL_TRUE, GL_TRUE, GL_TRUE, GL_TRUE);

  //  reset viewport to texture size.
  glViewport(0, 0, rect._width, rect._height);

  Math::Matrix4x4 projMatrix;
  projMatrix.orthogonalProject( rect._offsetx, rect._offsetx + rect._width, rect._offsety + rect._height, rect._offsety, -1000, 1000 );

  renderScript( pRenderScript, projMatrix, Math::Matrix4x4() );


  //  Set back viewport to initial values.
  glViewport(viewport[0], viewport[1], viewport[2], viewport[3]);

  //  Set back frame buffer object id.
  glBindFramebuffer(GL_FRAMEBUFFER, prevFboId);
  //glBindRenderbuffer(GL_RENDERBUFFER, prevRboId);
}

void RD_RendererGLES2::renderScript( const RD_RenderScriptPtr_t &pRenderScript, const Math::Matrix4x4 &projectionMatrix, const Math::Matrix4x4 &modelViewMatrix )
{
  pRenderScript->render(this, projectionMatrix, modelViewMatrix);
}

void RD_RendererGLES2::beginSpriteSheet( const RD_SpriteSheetPtr_t &pSpriteSheet )
{
  if ( !pSpriteSheet.get() )
    return;

  RD_SpriteSheetWeakPtr_t pWeakSpriteSheet(pSpriteSheet); // for comparison.
  Impl::TextureIdCol_t::const_iterator iTextureId = _i->_textureIds.find( pWeakSpriteSheet );

  if ( iTextureId == _i->_textureIds.end() )
  {
    //  Create texture id using sprite sheet image.
    IM_ImagePtr_t pImage = pSpriteSheet->image();
    if ( pImage.isValid() )
    {
      GLuint textureId;

      glGenTextures( 1, &textureId );
      glBindTexture( GL_TEXTURE_2D, textureId );

      glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
      glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
      glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
      glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);

      unsigned depth = pImage->depth();

      if ( depth == 4u )
        glTexImage2D( GL_TEXTURE_2D, 0, GL_RGBA, pImage->width(), pImage->height(), 0, GL_RGBA, GL_UNSIGNED_BYTE, pImage->data() );
      else if ( depth == 3u )
        glTexImage2D( GL_TEXTURE_2D, 0, GL_RGB, pImage->width(), pImage->height(), 0, GL_RGB, GL_UNSIGNED_BYTE, pImage->data() );

      //  Save in texture id collection
      _i->_textureIds.insert( STD_MakePair( pWeakSpriteSheet, textureId ) );

      //  Sprite sheet data has been loaded into GL.  Free unecessary resources.
      pSpriteSheet->freeSpriteSheetData();
    }
  }
  else
  {
    //  Reuse texture id.
    glBindTexture( GL_TEXTURE_2D, iTextureId->second );
  }

  if ( _i->_activeProgram )
  {
    _i->_activeProgram->setTextureSampler0( 0 );
  }
}

void RD_RendererGLES2::endSpriteSheet()
{
  glBindTexture( GL_TEXTURE_2D, 0 );
}

void RD_RendererGLES2::beginComposition( const RD_RenderScriptPlainPtr_t &pRenderScript )
{
  //  Create or retrieve shader program.
  Impl::ShaderProgramCol_t::const_iterator iProgram = _i->_shaders.find( Impl::ePlainShader );

  RD_ShaderProgramPlainGLES2 *program = static_cast<RD_ShaderProgramPlainGLES2*>(
      (iProgram == _i->_shaders.end()) ?
        _i->_shaders.insert( STD_MakePair( Impl::ePlainShader, new RD_ShaderProgramPlainGLES2 ) ).first->second :
        iProgram->second );

  //  Activate shader program if not already active.
  if ( _i->_activeProgram != program )
  {
    program->activate();
    _i->_activeProgram = program;
  }

  RD_RenderScriptWeakPtr_t pWeakRenderScript(pRenderScript.get()); // for comparison.

  const RD_RenderScriptPlain::VertexData *vertices = pRenderScript->vertices();
  size_t verticesCount = pRenderScript->verticesCount();

  const RD_RenderScriptPlain::Index_t *indices = pRenderScript->indices();
  size_t indicesCount = pRenderScript->indicesCount();

#ifdef ENABLE_VERTEX_BUFFER_OBJECT

#ifdef ENABLE_VERTEX_ARRAY_OBJECT
  GLuint vertexArrayId = 0;
#endif

  bool mapBuffers = pRenderScript->isRenderDirty();
  bool mapArrays = true;

  GLuint vertexBufferId = 0, indexBufferId = 0;

  Impl::BufferIdCol_t::const_iterator iBufferId = _i->_bufferIds.find( pWeakRenderScript );
  if ( iBufferId != _i->_bufferIds.end() )
  {
#ifdef ENABLE_VERTEX_ARRAY_OBJECT
    vertexArrayId = iBufferId->second._vertexArrayId;
    mapArrays = false;
#endif
    vertexBufferId = iBufferId->second._vertexBufferId;
    indexBufferId = iBufferId->second._indexBufferId;
  }
  else
  {
#ifdef ENABLE_VERTEX_ARRAY_OBJECT
    p_glGenVertexArrays( 1, &vertexArrayId );
#endif

    glGenBuffers( 1, &vertexBufferId );
    glGenBuffers( 1, &indexBufferId );

    Impl::BufferData data;
#ifdef ENABLE_VERTEX_ARRAY_OBJECT
    data._vertexArrayId = vertexArrayId;
#endif
    data._vertexBufferId = vertexBufferId;
    data._indexBufferId = indexBufferId;

    _i->_bufferIds.insert( STD_MakePair( pWeakRenderScript, data ) );
  }

#ifdef ENABLE_VERTEX_ARRAY_OBJECT
  p_glBindVertexArray( vertexArrayId );
#endif

  if ( mapArrays )
  {
    glBindBuffer(GL_ARRAY_BUFFER, vertexBufferId);
    if (mapBuffers)
      glBufferData(GL_ARRAY_BUFFER, sizeof(RD_RenderScriptPlain::VertexData) * verticesCount, vertices, GL_DYNAMIC_DRAW);

    // vertices
    glEnableVertexAttribArray(RD_ShaderProgramPlainGLES2::OGL_VERTEX_ATTRIB_POSITION);
    glVertexAttribPointer(RD_ShaderProgramPlainGLES2::OGL_VERTEX_ATTRIB_POSITION, 2, GL_FLOAT, GL_FALSE, sizeof(RD_RenderScriptPlain::VertexData), (GLvoid*) offsetof( RD_RenderScriptPlain::VertexData, _x ) );

    // colors
    glEnableVertexAttribArray(RD_ShaderProgramPlainGLES2::OGL_VERTEX_ATTRIB_COLOR);
    glVertexAttribPointer(RD_ShaderProgramPlainGLES2::OGL_VERTEX_ATTRIB_COLOR, 4, GL_UNSIGNED_BYTE, GL_FALSE, sizeof(RD_RenderScriptPlain::VertexData), (GLvoid*) offsetof( RD_RenderScriptPlain::VertexData, _color ) );

    // tex coords
    glEnableVertexAttribArray(RD_ShaderProgramPlainGLES2::OGL_VERTEX_ATTRIB_TEXCOORDS_0);
    glVertexAttribPointer(RD_ShaderProgramPlainGLES2::OGL_VERTEX_ATTRIB_TEXCOORDS_0, 2, GL_FLOAT, GL_FALSE, sizeof(RD_RenderScriptPlain::VertexData), (GLvoid*) offsetof( RD_RenderScriptPlain::VertexData, _u) );

    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, indexBufferId);
    if (mapBuffers)
      glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(RD_RenderScriptPlain::Index_t) * indicesCount, indices, GL_DYNAMIC_DRAW);
  }
  else if (mapBuffers)
  {
    glBindBuffer(GL_ARRAY_BUFFER, vertexBufferId);
    glBufferData(GL_ARRAY_BUFFER, sizeof(RD_RenderScriptPlain::VertexData) * verticesCount, vertices, GL_DYNAMIC_DRAW);

    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, indexBufferId);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(RD_RenderScriptPlain::Index_t) * indicesCount, indices, GL_DYNAMIC_DRAW);
  }

#else

  (void)verticesCount;

  (void)indices;
  (void)indicesCount;

  // vertices
  glEnableVertexAttribArray(RD_ShaderProgramPlainGLES2::OGL_VERTEX_ATTRIB_POSITION);
  glVertexAttribPointer(RD_ShaderProgramPlainGLES2::OGL_VERTEX_ATTRIB_POSITION, 2, GL_FLOAT, GL_FALSE, sizeof(RD_RenderScriptPlain::VertexData), &vertices->_x);

  // colors
  glEnableVertexAttribArray(RD_ShaderProgramPlainGLES2::OGL_VERTEX_ATTRIB_COLOR);
  glVertexAttribPointer(RD_ShaderProgramPlainGLES2::OGL_VERTEX_ATTRIB_COLOR, 4, GL_UNSIGNED_BYTE, GL_FALSE, sizeof(RD_RenderScriptPlain::VertexData), &vertices->_color);

  // tex coords
  glEnableVertexAttribArray(RD_ShaderProgramPlainGLES2::OGL_VERTEX_ATTRIB_TEXCOORDS_0);
  glVertexAttribPointer(RD_ShaderProgramPlainGLES2::OGL_VERTEX_ATTRIB_TEXCOORDS_0, 2, GL_FLOAT, GL_FALSE, sizeof(RD_RenderScriptPlain::VertexData), &vertices->_u);

#endif

  //  Disable writing to depth buffer to avoid z-fighting issues.
  glDepthMask(false);
}

void RD_RendererGLES2::endComposition( const RD_RenderScriptPlainPtr_t & /*pRenderScript*/ )
{
  glDepthMask(true);

#ifdef ENABLE_VERTEX_ARRAY_OBJECT
  p_glBindVertexArray( 0 );
#endif

  //  Must verify if vertex array object handles unbinding
  //  of vbos and vertex attrib arrays as well.

#ifdef ENABLE_VERTEX_BUFFER_OBJECT
  glBindBuffer(GL_ARRAY_BUFFER, 0);
  glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
#endif

  glDisableVertexAttribArray(RD_ShaderProgramPlainGLES2::OGL_VERTEX_ATTRIB_POSITION);
  glDisableVertexAttribArray(RD_ShaderProgramPlainGLES2::OGL_VERTEX_ATTRIB_COLOR);
  glDisableVertexAttribArray(RD_ShaderProgramPlainGLES2::OGL_VERTEX_ATTRIB_TEXCOORDS_0);
}


void RD_RendererGLES2::beginComposition( const RD_RenderScriptFxPtr_t &pRenderScript )
{
  //  Create or retrieve shader program.
  Impl::ShaderProgramCol_t::const_iterator iProgram = _i->_shaders.find( Impl::eFxShader );

  RD_ShaderProgramFxGLES2 *program = static_cast<RD_ShaderProgramFxGLES2*>(
      (iProgram == _i->_shaders.end()) ?
        _i->_shaders.insert( STD_MakePair( Impl::eFxShader, new RD_ShaderProgramFxGLES2 ) ).first->second :
        iProgram->second );

  //  Activate shader program if not already active.
  if ( _i->_activeProgram != program )
  {
    program->activate();
    _i->_activeProgram = program;
  }

  RD_RenderScriptWeakPtr_t pWeakRenderScript(pRenderScript.get()); // for comparison.

  const RD_RenderScriptFx::VertexData *vertices = pRenderScript->vertices();
  size_t verticesCount = pRenderScript->verticesCount();

  const RD_RenderScriptFx::Index_t *indices = pRenderScript->indices();
  size_t indicesCount = pRenderScript->indicesCount();

#ifdef ENABLE_VERTEX_BUFFER_OBJECT

#ifdef ENABLE_VERTEX_ARRAY_OBJECT
  GLuint vertexArrayId = 0;
#endif

  bool mapBuffers = pRenderScript->isRenderDirty();
  bool mapArrays = true;

  GLuint vertexBufferId = 0, indexBufferId = 0;

  Impl::BufferIdCol_t::const_iterator iBufferId = _i->_bufferIds.find( pWeakRenderScript );
  if ( iBufferId != _i->_bufferIds.end() )
  {
#ifdef ENABLE_VERTEX_ARRAY_OBJECT
    vertexArrayId = iBufferId->second._vertexArrayId;
    mapArrays = false;
#endif
    vertexBufferId = iBufferId->second._vertexBufferId;
    indexBufferId = iBufferId->second._indexBufferId;
  }
  else
  {
#ifdef ENABLE_VERTEX_ARRAY_OBJECT
    p_glGenVertexArrays( 1, &vertexArrayId );
#endif

    glGenBuffers( 1, &vertexBufferId );
    glGenBuffers( 1, &indexBufferId );

    Impl::BufferData data;
#ifdef ENABLE_VERTEX_ARRAY_OBJECT
    data._vertexArrayId = vertexArrayId;
#endif
    data._vertexBufferId = vertexBufferId;
    data._indexBufferId = indexBufferId;

    _i->_bufferIds.insert( STD_MakePair( pWeakRenderScript, data ) );
  }

#ifdef ENABLE_VERTEX_ARRAY_OBJECT
  p_glBindVertexArray( vertexArrayId );
#endif

  if ( mapArrays )
  {
    glBindBuffer(GL_ARRAY_BUFFER, vertexBufferId);
    if (mapBuffers)
      glBufferData(GL_ARRAY_BUFFER, sizeof(RD_RenderScriptFx::VertexData) * verticesCount, vertices, GL_DYNAMIC_DRAW);

    // vertices
    glEnableVertexAttribArray(RD_ShaderProgramFxGLES2::OGL_VERTEX_ATTRIB_POSITION);
    glVertexAttribPointer(RD_ShaderProgramFxGLES2::OGL_VERTEX_ATTRIB_POSITION, 2, GL_FLOAT, GL_FALSE, sizeof(RD_RenderScriptFx::VertexData), (GLvoid*) offsetof( RD_RenderScriptFx::VertexData, _x ) );

    // colors
    glEnableVertexAttribArray(RD_ShaderProgramFxGLES2::OGL_VERTEX_ATTRIB_COLOR);
    glVertexAttribPointer(RD_ShaderProgramFxGLES2::OGL_VERTEX_ATTRIB_COLOR, 4, GL_UNSIGNED_BYTE, GL_FALSE, sizeof(RD_RenderScriptFx::VertexData), (GLvoid*) offsetof( RD_RenderScriptFx::VertexData, _color ) );

    // tex coords
    glEnableVertexAttribArray(RD_ShaderProgramFxGLES2::OGL_VERTEX_ATTRIB_TEXCOORDS_0);
    glVertexAttribPointer(RD_ShaderProgramFxGLES2::OGL_VERTEX_ATTRIB_TEXCOORDS_0, 2, GL_FLOAT, GL_FALSE, sizeof(RD_RenderScriptFx::VertexData), (GLvoid*) offsetof( RD_RenderScriptFx::VertexData, _u0) );

    // fx params
    glEnableVertexAttribArray(RD_ShaderProgramFxGLES2::OGL_VERTEX_ATTRIB_FX_PARAMS_0);
    glVertexAttribPointer(RD_ShaderProgramFxGLES2::OGL_VERTEX_ATTRIB_FX_PARAMS_0, 4, GL_FLOAT, GL_FALSE, sizeof(RD_RenderScriptFx::VertexData), (GLvoid*) offsetof( RD_RenderScriptFx::VertexData, _fxParams0[0]) );

    // fx viewport
    glEnableVertexAttribArray(RD_ShaderProgramFxGLES2::OGL_VERTEX_ATTRIB_FX_VIEWPORT_0);
    glVertexAttribPointer(RD_ShaderProgramFxGLES2::OGL_VERTEX_ATTRIB_FX_VIEWPORT_0, 4, GL_FLOAT, GL_FALSE, sizeof(RD_RenderScriptFx::VertexData), (GLvoid*) offsetof( RD_RenderScriptFx::VertexData, _fxViewport0[0]) );

    // bone deformation
    glEnableVertexAttribArray(RD_ShaderProgramFxGLES2::OGL_VERTEX_ATTRIB_BONE_INDEX);
    glVertexAttribPointer(RD_ShaderProgramFxGLES2::OGL_VERTEX_ATTRIB_BONE_INDEX, 4, GL_FLOAT, GL_FALSE, sizeof(RD_RenderScriptFx::VertexData), (GLvoid*) offsetof( RD_RenderScriptFx::VertexData, _boneParams[0]) );

    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, indexBufferId);
    if (mapBuffers)
      glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(RD_RenderScriptFx::Index_t) * indicesCount, indices, GL_DYNAMIC_DRAW);
  }
  else if (mapBuffers)
  {
    glBindBuffer(GL_ARRAY_BUFFER, vertexBufferId);
    glBufferData(GL_ARRAY_BUFFER, sizeof(RD_RenderScriptFx::VertexData) * verticesCount, vertices, GL_DYNAMIC_DRAW);

    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, indexBufferId);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(RD_RenderScriptFx::Index_t) * indicesCount, indices, GL_DYNAMIC_DRAW);
  }

#else

  (void)verticesCount;

  (void)indices;
  (void)indicesCount;

  // vertices
  glEnableVertexAttribArray(RD_ShaderProgramFxGLES2::OGL_VERTEX_ATTRIB_POSITION);
  glVertexAttribPointer(RD_ShaderProgramFxGLES2::OGL_VERTEX_ATTRIB_POSITION, 2, GL_FLOAT, GL_FALSE, sizeof(RD_RenderScriptFx::VertexData), &vertices->_x);

  // colors
  glEnableVertexAttribArray(RD_ShaderProgramFxGLES2::OGL_VERTEX_ATTRIB_COLOR);
  glVertexAttribPointer(RD_ShaderProgramFxGLES2::OGL_VERTEX_ATTRIB_COLOR, 4, GL_UNSIGNED_BYTE, GL_FALSE, sizeof(RD_RenderScriptFx::VertexData), &vertices->_color);

  // tex coords
  glEnableVertexAttribArray(RD_ShaderProgramFxGLES2::OGL_VERTEX_ATTRIB_TEXCOORDS_0);
  glVertexAttribPointer(RD_ShaderProgramFxGLES2::OGL_VERTEX_ATTRIB_TEXCOORDS_0, 2, GL_FLOAT, GL_FALSE, sizeof(RD_RenderScriptFx::VertexData), &vertices->_u0);

  // fx params
  glEnableVertexAttribArray(RD_ShaderProgramFxGLES2::OGL_VERTEX_ATTRIB_FX_PARAMS_0);
  glVertexAttribPointer(RD_ShaderProgramFxGLES2::OGL_VERTEX_ATTRIB_FX_PARAMS_0, 4, GL_FLOAT, GL_FALSE, sizeof(RD_RenderScriptFx::VertexData), &vertices->_fxParams0[0] );

  // fx viewport
  glEnableVertexAttribArray(RD_ShaderProgramFxGLES2::OGL_VERTEX_ATTRIB_FX_VIEWPORT_0);
  glVertexAttribPointer(RD_ShaderProgramFxGLES2::OGL_VERTEX_ATTRIB_FX_VIEWPORT_0, 4, GL_FLOAT, GL_FALSE, sizeof(RD_RenderScriptFx::VertexData), &vertices->_fxViewport0[0] );

  // bone deformation
  glEnableVertexAttribArray(RD_ShaderProgramFxGLES2::OGL_VERTEX_ATTRIB_BONE_INDEX);
  glVertexAttribPointer(RD_ShaderProgramFxGLES2::OGL_VERTEX_ATTRIB_BONE_INDEX, 4, GL_FLOAT, GL_FALSE, sizeof(RD_RenderScriptFx::VertexData), &vertices->_boneParams[0] );

#endif

  //  Disable writing to depth buffer to avoid z-fighting issues.
  glDepthMask(false);
}

void RD_RendererGLES2::endComposition( const RD_RenderScriptFxPtr_t &/*pRenderScript*/ )
{
  glDepthMask(true);

#ifdef ENABLE_VERTEX_ARRAY_OBJECT
  p_glBindVertexArray( 0 );
#endif

  //  Must verify if vertex array object handles unbinding
  //  of vbos and vertex attrib arrays as well.

#ifdef ENABLE_VERTEX_BUFFER_OBJECT
  glBindBuffer(GL_ARRAY_BUFFER, 0);
  glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
#endif

  glDisableVertexAttribArray(RD_ShaderProgramFxGLES2::OGL_VERTEX_ATTRIB_POSITION);
  glDisableVertexAttribArray(RD_ShaderProgramFxGLES2::OGL_VERTEX_ATTRIB_COLOR);
  glDisableVertexAttribArray(RD_ShaderProgramFxGLES2::OGL_VERTEX_ATTRIB_TEXCOORDS_0);
  glDisableVertexAttribArray(RD_ShaderProgramFxGLES2::OGL_VERTEX_ATTRIB_FX_PARAMS_0);
  glDisableVertexAttribArray(RD_ShaderProgramFxGLES2::OGL_VERTEX_ATTRIB_FX_VIEWPORT_0);

}

void RD_RendererGLES2::beginBatch( const RD_RenderScriptFx::RenderBatch &renderBatch )
{
  RD_ShaderProgramFxGLES2 *program = static_cast<RD_ShaderProgramFxGLES2*>(_i->_activeProgram);
  program->setTextureSampler1( 0 );

  unsigned boneCount = renderBatch._uniformBoneMatrices.size();
  unsigned boneCountGPU = (boneCount > MAX_BONES_GPU) ? MAX_BONES_GPU : boneCount;
  program->setBoneMatrices( (const float*)&renderBatch._uniformBoneMatrices[0], boneCountGPU );
}

void RD_RendererGLES2::endBatch( const RD_RenderScriptFx::RenderBatch &/*renderBatch*/ )
{
}

void RD_RendererGLES2::renderVertices( unsigned short *indices, unsigned indexOffset, unsigned nIndices, unsigned /*vertexOffset*/, unsigned /*nVertices*/ )
{
  if ( nIndices > 0u )
  {
    //fprintf( stderr, "renderVertices() offset: %i nIndices: %i\n", offset, nIndices );

#ifdef ENABLE_VERTEX_BUFFER_OBJECT
    (void)indices;
    glDrawElements(GL_TRIANGLES, nIndices, GL_UNSIGNED_SHORT, (const GLvoid*)(indexOffset * sizeof(unsigned short)) );
#else
    glDrawElements(GL_TRIANGLES, nIndices, GL_UNSIGNED_SHORT, (const GLvoid*)(indices + indexOffset * sizeof(unsigned short)) );
#endif
  }
}

#endif /* SUPPORT_RENDERER_GLES2 || SUPPORT_RENDERER_GL */
