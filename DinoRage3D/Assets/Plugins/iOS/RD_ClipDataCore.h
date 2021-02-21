#ifndef _RD_CLIP_DATA_CORE_H_
#define _RD_CLIP_DATA_CORE_H_

#include "TV_NodeTreeView.h"

#include "MEM_Override.h"
#include "STD_Types.h"

class TR_NodeTree;
class IO_PersistentStore;

/*!
 *  @class RD_ClipDataCore
 *  Clip data container
 */
class RD_ClipDataCore
{
  MEM_OVERRIDE

public:

  RD_ClipDataCore();
  virtual ~RD_ClipDataCore();

  TV_NodeTreeViewPtr_t nodeTreeView( unsigned idx ) const;
  TV_NodeTreeViewPtr_t parentNodeTreeView( unsigned idx ) const;

  size_t count() const;

  void beginClipData();
  void addNodeTree ( TR_NodeTree *, const STD_String &skeletonName, const STD_String &parentSkeletonName = "", const STD_String &parentBoneName = ""  );
  void addSoundEvent( const STD_String &soundName, float startFrame );
  void endClipData();

  void store( IO_PersistentStore &store ) const;
  bool load( IO_PersistentStore &store );

  bool  fxEnabled() const;
  float totalDuration() const;

private:

  void storeHeader( IO_PersistentStore &store ) const;
  bool loadHeader( IO_PersistentStore &store ) const;

  void updateFxFlag();
  void eraseAll();

private:

  class Impl;
  Impl *_i;
};

#endif /* _RD_CLIP_DATA_CORE_H_ */
