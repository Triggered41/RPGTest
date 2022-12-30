package myExt;

import java.util.Collection;

import com.smartfoxserver.v2.core.ISFSEvent;
import com.smartfoxserver.v2.core.ISFSEventParam;
import com.smartfoxserver.v2.entities.SFSUser;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseServerEventHandler;

public class Zoner extends BaseServerEventHandler {
    public void handleServerEvent(ISFSEvent ev)
    {
        ISFSObject obj = new SFSObject();
        Collection<User> col = getParentExtension().getParentZone().getUserList();//All players in zone
        SFSUser o = (SFSUser) col.toArray()[col.size()-1]; // Last player that joined
        obj.putText("msg", o.getName() + "joined");
        for (User user : col) {
            send("NewUser", obj, user);
        }
        
        
    }    
}
