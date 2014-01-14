using System.Collections;
using INet;
public interface NetworkListener {
	void receiveCmd(Command cmd);

	void onError();

	void onConnected();

	void onConnectFailure();
}
