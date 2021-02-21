#ifndef _RD_SPRITE_SHEET_H_
#define _RD_SPRITE_SHEET_H_

#include "RD_RenderObjectManager.h"

#include "UT_SharedWeakPtr.h"
#include "STD_Types.h"

#include "RD_SpriteSheetCore.h"
#include "IM_Image.h"

// typedef void* RD_TexturePtr_t

class RD_SpriteSheet;
typedef UT_SharedPtr< RD_SpriteSheet > RD_SpriteSheetPtr_t;
typedef UT_SharedWeakPtr< RD_SpriteSheet > RD_SpriteSheetWeakPtr_t;

/*!
 *  @struct RD_SpriteSheetKey
 *  Tuple to identify a unique sprite sheet.
 */
struct RD_SpriteSheetKey
{
  RD_SpriteSheetKey( const STD_String &projectFolder, const STD_String &sheetName, const STD_String &resolutionName ) :
    _projectFolder(projectFolder),
    _sheetName(sheetName),
    _resolutionName(resolutionName)
  {
  }

  bool operator< ( const RD_SpriteSheetKey &key ) const
  {
    //  To be reviewed, sorting could probably be improved if
    //  it becomes a bottleneck.
    if ( _projectFolder.compare(key._projectFolder) == 0 )
    {
      if ( _sheetName.compare(key._sheetName) == 0 )
      {
        return _resolutionName < key._resolutionName;
      }

      return ( _sheetName < key._sheetName );
    }

    return _projectFolder < key._projectFolder;
  }

  STD_String _projectFolder;
  STD_String _sheetName;
  STD_String _resolutionName;
};

typedef RD_RenderObjectManager<RD_SpriteSheetKey, RD_SpriteSheet> RD_SpriteSheetManager;

/*!
 *  @class RD_SpriteSheet
 *  Sprite Sheet data structure.
 */
class RD_SpriteSheet : public UT_SharedWeakBase
{
public:

  typedef RD_SpriteSheetCore::Rect Rect;
  typedef RD_SpriteSheetCore::SpriteData SpriteData;

public:
  static RD_SpriteSheetPtr_t create( const RD_SpriteSheetKey &key );
  static RD_SpriteSheetPtr_t createOrLoad( const RD_SpriteSheetKey &key );

private:
  RD_SpriteSheet( const STD_String &projectFolder, RD_SpriteSheetCore *spriteSheet );

public:
  virtual ~RD_SpriteSheet();

  bool buildSpriteSheet();
  void freeSpriteSheetData();

  const SpriteData *sprite( const STD_String &name ) const;

  bool rect( const STD_String &name, Rect &sprite ) const;

  bool matrix( const STD_String &name, Math::Matrix4x4 &matrix ) const;

  const STD_String &projectFolder() const;
  STD_String        sheetFilename() const;
  const STD_String &sheetName() const;
  const STD_String &sheetResolution() const;

  bool uvs( const STD_String &name, float &u1, float &v1, float &u2, float &v2 ) const;
  bool uvs( const STD_String &name, float uvs[4] ) const;

  bool uvs( const Rect &rect, float &u1, float &v1, float &u2, float &v2 ) const;
  bool uvs( const Rect &rect, float uvs[4] ) const;

  bool hasImage() const;
  const IM_ImagePtr_t &image() const;

  unsigned width() const;
  unsigned height() const;

private:

  class Impl;
  Impl *_i;
};

#endif /* _RD_SPRITE_SHEET_H_ */
