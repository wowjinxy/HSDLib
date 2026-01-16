using System;
using System.Numerics;

namespace HSDRawViewer.Extensions
{
    public static class NumericsExtensions
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static Quaternion LookAtZ(Vector3 from, Vector3 to)
        {
            Vector3 forward = Vector3.Normalize(to - from);

            // Handle degenerate case
            if (forward.LengthSquared() < 1e-6f)
                return Quaternion.Identity;

            Vector3 up = Vector3.UnitY;

            // If forward is parallel to up, choose a different up
            if (MathF.Abs(Vector3.Dot(forward, up)) > 0.999f)
                up = Vector3.UnitX;

            Vector3 right = Vector3.Normalize(Vector3.Cross(up, forward));
            up = Vector3.Cross(forward, right);

            // Rotation matrix (Z forward, Y up)
            Matrix4x4 m = new Matrix4x4(
                right.X, right.Y, right.Z, 0,
                up.X, up.Y, up.Z, 0,
                forward.X, forward.Y, forward.Z, 0,
                0, 0, 0, 1
            );

            return Quaternion.CreateFromRotationMatrix(m);
        }
    }
}
