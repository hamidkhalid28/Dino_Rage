#ifndef _RD_SPRITE_SHEET_CORE_H_
#define _RD_SPRITE_SHEET_CORE_H_

#include "MT_Matrix4x4.h"
#include "MEM_Override.h"
#include "STD_Containers.h"
#include "STD_Types.h"

#include <string>

class RD_SpriteSheetCore;
class IO_PersistentStore;

/*!
 * @class RD_SpriteSheetCore
 * Maps single texture into several sprites.
 */
class RD_SpriteSheetCore
{
  MEM_OVERRIDE

  friend class XML_SpriteSheetSaxParser;

public:

  struct Rect
  {
    MEM_OVERRIDE

    int _x, _y;
    int _w, _h;
  };

  struct SpriteData
  {
    MEM_OVERRIDE

    Rect            _rect;
    Math::Matrix4x4 _matrix;
  };

  typedef STD_Map< STD_String, SpriteData > SpriteCol_t;

public:

  RD_SpriteSheetCore();
  virtual ~RD_SpriteSheetCore();

  const STD_String &sheetFilename() const;
  const STD_String &sheetName() const;
  const STD_String &sheetResolution() const;

public:

  const SpriteData *sprite( const STD_String &name ) const;

  bool rect( const STD_String &name, Rect &sprite ) const;
  bool matrix( const STD_String &name, Math::Matrix4x4 &matrix ) const;

  size_t size() const;

  bool empty() const;

  void store( IO_PersistentStore &store ) const;
  bool load( IO_PersistentStore &store );

public:

  SpriteCol_t::const_iterator begin() const;
  SpriteCol_t::const_iterator end() const;

protected:

  void storeHeader( IO_PersistentStore &store ) const;
  bool loadHeader( IO_PersistentStore &store ) const;

  void addSprite( const STD_String &name, const Rect &rect, float offsetX, float offsetY, float scaleX, float scaleY );
  void addSprite( const STD_String &name, const Rect &rect, const Math::Matrix4x4 &matrix );

private:

  STD_String   _projectFolder;
  STD_String   _sheetFilename;
  STD_String   _sheetName;
  STD_String   _sheetResolution;

  SpriteCol_t  _sprites;

};

#endif /* _RD_SPRITE_SHEET_CORE_H_ */
