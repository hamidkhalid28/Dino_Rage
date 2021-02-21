#ifndef _RD_CLIP_DATA_H_
#define _RD_CLIP_DATA_H_

#include "RD_RenderObjectManager.h"

#include "UT_ShareBaseSafe.h"
#include "UT_SharedPtr.h"
#include "STD_Types.h"

#include "RD_ClipDataCore.h"

class TR_NodeTree;

class RD_ClipData;
typedef UT_SharedPtr<RD_ClipData> RD_ClipDataPtr_t;

typedef RD_RenderObjectManager<STD_String, RD_ClipData> RD_ClipDataManager;

class RD_ClipData : public UT_ShareBaseSafe
{
public:

  RD_ClipData( RD_ClipDataCore *pClipData );
  virtual ~RD_ClipData();

  TV_NodeTreeViewPtr_t nodeTreeView( unsigned idx ) const;
  TV_NodeTreeViewPtr_t parentNodeTreeView( unsigned idx ) const;

  size_t count() const;

  bool  fxEnabled() const;
  float totalDuration() const;

private:

  class Impl;
  Impl *_i;
};

#endif /* _RD_CLIP_DATA_H_ */
