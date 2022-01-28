--code by 赫斯基皇
--https://space.bilibili.com/455965619
--https://github.com/Heskey0

require "Common.UI.UIComponent"
require "Common.UI.Countdown"
require("Common.UI.ItemListCreator")

--管理器--
local Game = {}
local Ctrls = {}

function LuaMain()
	print("logic start")
	UpdateManager:GetInstance():Startup()	--将UpdateHandle加入到UpdateBeat，之后调用AddUpdate对UpdateHandle执行操作

	Game:OnInitOK()
end

function Game:OnInitOK()
	print('Game.lua[Game.OnInitOK()]')
	--[[
	GlobalEventSystem.Init()
	Game:InitLuaPool()
	Game:InitUI()
	Game:InitControllers()
	--]]
	
	local __update_handle = BindCallback(self, function() 
			CS.TimerMgr.Instance:Loop()
			end)	--生成一个listener句柄
	UpdateManager:GetInstance():AddUpdate(__update_handle)
end
