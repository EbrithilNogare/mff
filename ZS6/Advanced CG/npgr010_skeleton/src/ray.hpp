#pragma once

#include <vector>
#include <cmath>
#include "math.hpp"

//////////////////////////////////////////////////////////////////////////
// Ray casting
struct Ray
{
    Ray()
    {}

    Ray(const Vec3f& aOrg,
        const Vec3f& aDir,
        float aTMin
    ) :
        origin(aOrg),
        direction(aDir),
        offset(aTMin)
    {}

    Vec3f origin;  //!< Ray origin
    Vec3f direction;  //!< Ray direction
    float offset; //!< Minimal distance to intersection
};

struct Intersection
{
    Intersection()
    {}

    Intersection(float aMaxDist):distance(aMaxDist)
    {}

    float distance;    //!< Distance to closest intersection (serves as ray.tmax)
    int   materialID;   //!< ID of intersected material
    int   lightID; //!< ID of intersected light (if < 0, then none)
    Vec3f normal;  //!< Normal vector at the intersection
};
