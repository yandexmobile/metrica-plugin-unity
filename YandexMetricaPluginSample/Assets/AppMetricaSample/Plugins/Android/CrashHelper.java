interface CrashHelperCallback {
    void invoke();
}

class CrashHelper {

	public static void crash(String message) throws IllegalArgumentException {
		throw new IllegalArgumentException(message);
	}

	public static void crashInOtherLine(String message) throws IllegalArgumentException {
		throw new IllegalArgumentException(message);
	}

	public static void otherCrash(String message) {
		throw new RuntimeException(message);
	}

	public static void crashInOtherThread(final String message) {
		new Thread(new Runnable() {
			public void run() {
				throw new RuntimeException(message);
			}
		}).start();
	}

	public static void callCs(CrashHelperCallback callback) {
        callback.invoke();
    }
}
