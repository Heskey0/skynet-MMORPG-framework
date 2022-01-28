Joystick=require"Joystick"
TouchScene=require"TouchScene"
--摇杆
local _joystick
--与场景的点击交互
local _touchScene 
--功能大厅
local _functionHall

local FightUIMgr=BaseClass()

FightUIMgr.Instance=nil

function FightUIMgr:GetInstance()
	if FightUIMgr.Instance == nil then
		FightUIMgr.Instance=FightUIMgr.New()
	end
	return FightUIMgr.Instance
end

local function Init()
	if _joystick==nil then
		_joystick=Joystick.New()
	end
	if _touchScene==nil then
		_touchScene=TouchScene.New()
	end
end
