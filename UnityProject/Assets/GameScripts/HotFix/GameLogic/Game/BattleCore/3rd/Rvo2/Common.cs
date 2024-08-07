using UnityEngine;
namespace Rvo2
{
  

    public  class Vector2 {
        public float x;
        public float y;

        public Vector2(float x, float y) {
            this.x = x;
            this.y = y;
        }

        public Vector2 Plus(Vector2 vector) {
            return new Vector2(this.x + vector.x, this.y + vector.y);
        }

        public Vector2 Minus(Vector2 vector) {
            return new Vector2(this.x - vector.x, this.y - vector.y);
        }

        public float Multiply(Vector2 vector) {
            return this.x * vector.x + this.y * vector.y;
        }

        public Vector2 Scale(float k) {
            return new Vector2(this.x * k, this.y * k);
        }

        public Vector2 Set(float x, float y) {
            this.x = x;
            this.y = y;
            return this;
        }

        public Vector2 Copy(Vector2 v) {
            this.x = v.x;
            this.y = v.y;
            return this;
        }

        public Vector2 Clone() {
            return new Vector2(this.x, this.y);
        }

        public Vector2 Substract(Vector2 target, Vector2 other) {
            target.x -= other.x;
            target.y -= other.y;
            return target;
        }

        public float LengthSqr() {
            return this.x * this.x + this.y * this.y;
        }
        
        //乘法
        public static Vector2 operator *(Vector2 v, float f) {
            return new Vector2(v.x * f, v.y * f);
        }
        
        //Dot
        public static float Dot(Vector2 v1, Vector2 v2) {
            return v1.x * v2.x + v1.y * v2.y;
        }
        //除法
        public static Vector2 operator /(Vector2 v, float f) {
            return new Vector2(v.x / f, v.y / f);
        }
        //Normalize
        public  Vector2 Normalize() {
            
            float num = Mathf.Sqrt(x * x + y * y);
            if (num > 9.99999974737875E-06) {
                x /= num;
                y /= num;
            }
            return new Vector2(0, 0);
        }
    }

    public class Obstacle {
        public Obstacle Next;
        public Obstacle Previous;
        public Vector2 Direction;
        public Vector2 Point;
        public int Id;
        public bool Convex;
    }

    public class Line {
        public Vector2 point;
        public Vector2 direction;
    }

    public class KeyValuePair<K, V> {
        public K key;
        public V value;

        public KeyValuePair(K key, V value) {
            this.key = key;
            this.value = value;
        }
    }

    public class RVOMath {
        public static float RVO_EPSILON = 0.00001f;

        public static float AbsSq(Vector2 v) {
            return v.Multiply(v);
        }

        public static Vector2 Normalize(Vector2 v) {
            return v.Scale(1 / RVOMath.Abs(v));
        }

        public static float DistSqPointLineSegment(Vector2 vector1, Vector2 vector2, Vector2 vector3) {
            Vector2 aux1 = vector3.Minus(vector1);
            Vector2 aux2 = vector2.Minus(vector1);

            float r = aux1.Multiply(aux2) / RVOMath.AbsSq(aux2);

            if (r < 0) {
                return RVOMath.AbsSq(aux1);
            }
            else if (r > 1) {
                return RVOMath.AbsSq(vector3.Minus(vector2));
            }
            else {
                return RVOMath.AbsSq(vector3.Minus(vector1.Plus(aux2.Scale(r))));
            }
        }

        public static float Sqr(float p) {
            return p * p;
        }

        public static float Det(Vector2 v1, Vector2 v2) {
            return v1.x * v2.y - v1.y * v2.x;
        }

        public static float Abs(Vector2 v) {
            return Mathf.Sqrt(RVOMath.AbsSq(v));
        }

        public static float LeftOf(Vector2 a, Vector2 b, Vector2 c) {
            return RVOMath.Det(a.Minus(c), b.Minus(a));
        }
    }

}