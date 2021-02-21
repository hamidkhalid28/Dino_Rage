#ifndef _XML_PROJECT_LOADER_H_
#define _XML_PROJECT_LOADER_H_

#include "MEM_Override.h"
#include "STD_Containers.h"
#include "STD_Types.h"

#include <string>

class TR_NodeTree;
class RD_ClipDataCore;
class RD_SpriteSheetCore;

/*!
 *  @namespace XML_ProjectLoader
 */
namespace XML_ProjectLoader
{
  typedef STD_Vector< STD_String > StringCol_t;

  typedef STD_Pair< STD_String, STD_String > StringPair_t;
  typedef STD_Vector< StringPair_t > StringPairCol_t;

  typedef STD_Pair< STD_String, float > SoundEvent_t;
  typedef STD_Vector< SoundEvent_t > SoundEventCol_t;

  void loadStageClipNames( const STD_String &projectFolder, StringCol_t &clipNames );

  void loadStageClip( const STD_String &projectFolder, const STD_String &clipName, RD_ClipDataCore *clip );
  void loadSkeleton( const STD_String &projectFolder, const STD_String &skeletonName, TR_NodeTree *nodeTree );
  void loadAnimation( const STD_String &projectFolder, const STD_String &animationName, TR_NodeTree *nodeTree );
  void loadDrawingAnimation( const STD_String &projectFolder, const STD_String &drawingAnimationName, TR_NodeTree *nodeTree );

  void loadSpriteSheetNames( const STD_String &projectFolder, StringPairCol_t &sheetNames );
  void loadSpriteSheetResolutionNames( const STD_String &projectFolder, const STD_String &sheetName, StringPairCol_t &resolutionNames );

  void loadSpriteSheet( const STD_String &projectFolder, const STD_String &sheetName, const STD_String &sheetResolution, RD_SpriteSheetCore *sheet );
  void loadSpriteSheet( const STD_String &projectFolder, const STD_String &sheetName, RD_SpriteSheetCore *sheet );
}

#endif /* _XML_PROJECT_LOADER_H_ */
