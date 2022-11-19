#pragma once

#include <vector>
#include <cmath>
#include <utility>
#include "math.hpp"

#define EPSILON_COSINE 1e-6f
#define EPSILON_RAY    1e-3f

// sRGB luminance
float Luminance(const Vec3f& aRGB)
{
    return 0.212671f * aRGB.x +
        0.715160f * aRGB.y +
        0.072169f * aRGB.z;
}

Vec3f rotateByAngle(Vec3f in, Vec3f up, Vec3f rnd) {
    Vec3f tangent = Normalize(rnd * 2 - 1);
    Vec3f bitangent = Cross(tangent, up);
    tangent = Cross(bitangent, up);

    return tangent * in.x + bitangent * in.y + up * in.z;
}

// reflect vector through (0,0,1)
Vec3f ReflectLocal(const Vec3f& aVector)
{
    return Vec3f(-aVector.x, -aVector.y, aVector.z);
}

//////////////////////////////////////////////////////////////////////////
// Utilities for converting PDF between Area (A) and Solid angle (W)
// WtoA = PdfW * cosine / distance_squared
// AtoW = PdfA * distance_squared / cosine

float PdfWtoA(
    const float aPdfW,
    const float aDist,
    const float aCosThere)
{
    return aPdfW * std::abs(aCosThere) / Sqr(aDist);
}

float PdfAtoW(
    const float aPdfA,
    const float aDist,
    const float aCosThere)
{
    return aPdfA * Sqr(aDist) / std::abs(aCosThere);
}
