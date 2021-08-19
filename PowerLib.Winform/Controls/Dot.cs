using System.Drawing;

using PowerLib.Utilities;

namespace PowerLib.Winform.Controls
{
    /// <summary>
    /// 表示一个"点"
    /// </summary>
    internal sealed class Dot
    {
        #region 字段/属性

        // 圆心
        private readonly PointF _circleCenter;
        // 半径
        private readonly float _circleRadius;

        /// <summary>
        /// 当前帧绘图坐标，在每次DoAction()时重新计算
        /// </summary>
        public PointF Location;

        // 点相对于圆心的角度，用于计算点的绘图坐标
        private int _angle;
        // 透明度
        private int _opacity;
        // 动画进度
        private int _progress;
        // 速度
        private int _speed;

        /// <summary>
        /// 透明度
        /// </summary>
        public int Opacity => _opacity < MIN_OPACITY ? MIN_OPACITY : _opacity > MAX_OPACITY ? MAX_OPACITY : _opacity;

        #endregion

        #region 常量

        // 最小/最大速度
        private const int MIN_SPEED = 2;
        private const int MAX_SPEED = 11;

        // 出现区的相对角度        
        private const int APPEAR_ANGLE = 90;
        // 减速区的相对角度
        private const int SLOW_ANGLE = 225;
        // 加速区的相对角度
        private const int QUICK_ANGLE = 315;

        // 最小/最大角度
        private const int MIN_ANGLE = 0;
        private const int MAX_ANGLE = 360;

        // 淡出速度
        private const int ALPHA_SUB = 25;

        // 最小/最大透明度
        private const int MIN_OPACITY = 0;
        private const int MAX_OPACITY = 255;

        #endregion 常量

        #region 构造器

        public Dot(PointF circleCenter, float circleRadius)
        {
            Reset();
            _circleCenter = circleCenter;
            _circleRadius = circleRadius;
        }

        #endregion 构造器

        #region 方法

        /// <summary>
        /// 重新计算当前帧绘图坐标
        /// </summary>
        private void ReCalcLocation()
        {
            Location = GraphicsUtility.GetDotLocationByAngle(_circleCenter, _circleRadius, _angle);
        }

        /// <summary>
        /// 点动作
        /// </summary>
        public void DotAction()
        {
            switch (_progress)
            {
                case 0:
                {
                    _opacity = MAX_OPACITY;
                    AddSpeed();
                    if (_angle + _speed >= SLOW_ANGLE && _angle + _speed < QUICK_ANGLE)
                    {
                        _progress = 1;
                        _angle = SLOW_ANGLE - _speed;
                    }
                }
                    break;
                case 1:
                {
                    SubSpeed();
                    if (_angle + _speed >= QUICK_ANGLE || _angle + _speed < SLOW_ANGLE)
                    {
                        _progress = 2;
                        _angle = QUICK_ANGLE - _speed;
                    }
                }
                    break;
                case 2:
                {
                    AddSpeed();
                    if (_angle + _speed >= SLOW_ANGLE && _angle + _speed < QUICK_ANGLE)
                    {
                        _progress = 3;
                        _angle = SLOW_ANGLE - _speed;
                    }
                }
                    break;
                case 3:
                {
                    SubSpeed();
                    if (_angle + _speed >= QUICK_ANGLE && _angle + _speed < MAX_ANGLE)
                    {
                        _progress = 4;
                        _angle = QUICK_ANGLE - _speed;
                    }
                }
                    break;
                case 4:
                {
                    SubSpeed();
                    if (_angle + _speed >= MIN_ANGLE && _angle + _speed < APPEAR_ANGLE)
                    {
                        _progress = 5;
                        _angle = MIN_ANGLE;
                    }
                }
                    break;
                case 5:
                {
                    AddSpeed();
                    FadeOut();
                }
                    break;
            }

            // 移动
            _angle = _angle >= MAX_ANGLE - _speed ? MIN_ANGLE : _angle + _speed;
            // 重新计算坐标
            ReCalcLocation();
        }

        // 淡出
        private void FadeOut()
        {
            if ((_opacity -= ALPHA_SUB) <= 0)
                _angle = APPEAR_ANGLE;
        }

        // 重置状态
        public void Reset()
        {
            _angle = APPEAR_ANGLE;
            _speed = MIN_SPEED;
            _progress = 0;
            _opacity = 1;
        }

        // 加速
        private void AddSpeed()
        {
            if (++_speed >= MAX_SPEED) _speed = MAX_SPEED;
        }

        // 减速
        private void SubSpeed()
        {
            if (--_speed <= MIN_SPEED) _speed = MIN_SPEED;
        }

        #endregion 方法
    }
}