return [[
.vector3{
    x 0 : double
    y 1 : double
    z 2 : double
}

.select_role_info{
    name 0 : string
    model_id 1 : integer
}

.create_scene_role{
    this_id 0 : integer
    name 1 : string
    model_id 2 : integer
    pos 3 : vector3
    face_to 4 : vector3
}

.create_npc_info{
    this_id 0 : integer
    model_id 1 : integer
    name 2 : string
    pos 3 : vector3
}

account{
    request{
        account 0 : integer
        password 1 : string
    }
    response{
        # RoleListCmd
        role_list 0 : *select_role_info
    }
}

select_role{
    request{
        index 0 : integer
    }
    response{
        # CreateSceneRoleCmd
        role 0 : create_scene_role
        # MainRoleThisidCmd
        this_id 1 : integer
        # EnterMapCmd
        enter_map 2 : integer
        # CreateSceneNpcCmd
        npc_list 3 : *create_npc_info
    }
}

]]