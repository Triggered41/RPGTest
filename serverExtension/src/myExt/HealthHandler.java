package myExt;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class HealthHandler extends BaseClientRequestHandler {

    public void handleClientRequest(User player, ISFSObject obj)
    {
        trace("Health Update");
        ISFSObject object = new SFSObject();
        object.putText("msg", "Health was updates");
        send("res", object, player);
        
    }

    
}
