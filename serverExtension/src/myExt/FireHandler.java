package myExt;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class FireHandler extends BaseClientRequestHandler {

    public void handleClientRequest(User player, ISFSObject obj)
    {
        obj.putText("Shooter", player.getName());
        send("Fire", obj, getParentExtension().getParentZone().getRoomByName("roomy").getUserList());   
    }

    
}
