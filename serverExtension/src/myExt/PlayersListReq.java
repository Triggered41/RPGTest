package myExt;


import java.util.ArrayList;
import java.util.Collection;
import java.util.List;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class PlayersListReq extends BaseClientRequestHandler{
    public void handleClientRequest(User player, ISFSObject obj)
    {
        trace("List Requested");
        List<User> users = getParentExtension().getParentZone().getRoomByName("roomy").getUserList();
        Collection<String> names = new ArrayList<String>();
        for (User user : users) {
            names.add(user.getName());
        }
        trace("Players: ", names);
        obj.putUtfStringArray("List", names);
        send("PlayersList", obj, player);
    }
}
