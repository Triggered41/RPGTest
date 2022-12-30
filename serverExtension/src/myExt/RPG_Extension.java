package myExt;

import com.smartfoxserver.v2.core.SFSEventType;
import com.smartfoxserver.v2.extensions.SFSExtension;

public class RPG_Extension extends SFSExtension{
    
    public void init()
    {
        trace("Extension Started");
        addRequestHandler("health", HealthHandler.class);
        addRequestHandler("PlayersListReq", PlayersListReq.class);
        addRequestHandler("Fire", FireHandler.class);

        addEventHandler(SFSEventType.USER_LOGIN, LoginHandler.class);
        addEventHandler(SFSEventType.USER_JOIN_ZONE, Zoner.class);
    }
}
