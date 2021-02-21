
#include "RD_RenderScript.h"

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#if 0
#pragma mark - RD_RenderScript
#endif

RD_RenderScript::RD_RenderScript()
{
}

RD_RenderScript::~RD_RenderScript()
{
}

void RD_RenderScript::deallocateConvexHull( BoundingVertex *convexHullArray )
{
  delete [] convexHullArray;
}

void RD_RenderScript::deallocatePolygons( BoundingVertex *polygonArray, int *subPolygonArray )
{
  delete [] polygonArray;
  delete [] subPolygonArray;
}

