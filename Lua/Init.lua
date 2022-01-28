--初始化

Mathf		= require "Common.UnityEngine.Mathf"
Vector2		= require "Common.UnityEngine.Vector2"
Vector3 	= require "Common.UnityEngine.Vector3"
Vector4		= require "Common.UnityEngine.Vector4"
Quaternion	= require "Common.UnityEngine.Quaternion"
Color		= require "Common.UnityEngine.Color"
Ray			= require "Common.UnityEngine.Ray"
Bounds		= require "Common.UnityEngine.Bounds"
RaycastHit	= require "Common.UnityEngine.RaycastHit"
Touch		= require "Common.UnityEngine.Touch"
LayerMask	= require "Common.UnityEngine.LayerMask"
Plane		= require "Common.UnityEngine.Plane"
Time		= require "Common.UnityEngine.Time"
Object		= require "Common.UnityEngine.Object"

require("Common/BaseClass")
require("Common.Util.util")
require("Common.Util.LuaUtil")
list = require("Common.Util.list")
require("Common.Util.event")
require("Common.Util.Timer")
UpdateManager = require "Common.UpdateManager"
require "Common.GlobalEventSystem"


--顺序无关的
require("Common.Util.Functor")


UpdateManager = require "Common.UpdateManager"
