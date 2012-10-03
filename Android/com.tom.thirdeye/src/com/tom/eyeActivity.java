package com.tom;

import java.io.IOException;
import java.io.InputStream;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.ServerSocket;
import java.net.Socket;
import java.net.SocketException;
import java.util.Timer;
import java.util.TimerTask;

import com.tom.eye.R;
import android.app.Activity;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.os.Build;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.util.DisplayMetrics;
import android.view.GestureDetector;
import android.view.GestureDetector.OnGestureListener;
import android.view.MotionEvent;
import android.widget.ImageView;
import android.widget.Toast;

public class eyeActivity extends Activity implements OnGestureListener{
	private static ServerSocket imageSocket = null;
	private static DatagramSocket eventSocket = null;
	private InetAddress senderAddress = null;
	private String info = "";
	private String msg = "";
	private static boolean runningOnBackground = false;
	private static boolean reportedError = false;
	private static boolean hasBegun = false;
	
	private final int CLICK = 1;
	private final int RIGHT_CLICK = 2;
	
	private GestureDetector mGestureDetector;
	
    /** Called when the activity is first created. */
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.main);
        imageView1 = (ImageView)findViewById(R.id.imageView1);
        handsUp();
        shutdownImageSocket();
        startSyc();
        //info += "create";
        //showInfo();
        runningOnBackground = false;
        reportedError = false;
        hasBegun = false;
        
        //regTouchEventListener();
        mGestureDetector = new GestureDetector(this);
    }
    
    private void sendTouchEvent(int type, MotionEvent e)
    {
    	try
    	{
    		if(null != eventSocket)
    		{
    			try {
					eventSocket.close();
				} catch (Exception ex) {
					// TODO Auto-generated catch block
					ex.printStackTrace();
				}
    			eventSocket = null;
    		}
    		if(null != senderAddress)
    		{
    			eventSocket = new DatagramSocket();
		        String message = String.valueOf(type)
		        + "|" + String.valueOf((int)e.getX())
		        + "|" + String.valueOf((int)e.getY());
		        byte[]data = message.getBytes();
		        DatagramPacket thePacket =new DatagramPacket(data, data.length, senderAddress, 9098);
		        eventSocket.send(thePacket);
    		}
    	}
    	catch(Exception ex)
    	{
    		msg = "exception" + ex.getMessage();
    	}
    }
    
    private String getInfo()
    {
    	String model = Build.MODEL;
    	DisplayMetrics dm = new DisplayMetrics();
    	getWindowManager().getDefaultDisplay().getMetrics(dm);
    	String result = String.valueOf(dm.widthPixels)
    	+ "|" + String.valueOf(dm.heightPixels)
    	+ "|" + model;
    	return result;
    }
    
    private void handsUp()
    {
    	try
    	{
	    	InetAddress receiveHost = InetAddress.getByName("255.255.255.255");   
	        DatagramSocket theSocket = new DatagramSocket(); 
	        String message = getInfo(); 
	        byte[]data  = message.getBytes();
	        DatagramPacket thePacket =new DatagramPacket(data,data.length,receiveHost,9095); 
	        theSocket.send(thePacket); 
    	}
    	catch(Exception ex)
    	{
    		
    	}
    }
    
    private void notifyRefreshDone()
    {
    	if(null == senderAddress || null == imageSocket)
    	{
    		return;
    	}
    	try
    	{
    		DatagramSocket socket = new DatagramSocket(); 
	        String message = "done";//getSizeInfo(); 
	        byte[]data  = message.getBytes();
	        DatagramPacket thePacket =new DatagramPacket(data,data.length, senderAddress, 9097); 
	        socket.send(thePacket);
    	}
    	catch(Exception ex)
    	{
    		
    	}
    }
    
	private void startSyc() {
		Timer timer = new Timer();
		TimerTask task = new TimerTask() {
			public void run() {
				cnt++;
				updateImage();
			}
		};
		timer.schedule(task, 0, 20);
	}
	
	private void updateImage()
	{
		/*
		try {
			if(null == imageSocket)
			{
				imageSocket= new ServerSocket(9096);
				imageSocket.setSoTimeout(20);
			}

			Socket socket=imageSocket.accept();
			if(null == senderAddress)
			{
				senderAddress = socket.getInetAddress();
			}
			InputStream ips = socket.getInputStream();
			image = BitmapFactory.decodeStream(ips);
			Message msg = new Message();
			msg.what = 2;
			handler.sendMessage(msg);
		} catch (Exception e) {
			//shutdownImageSocket();
			Toast.makeText(this, "Cannot receive data ", duration)
		}
		*/
		int code = 0;
		try {
			if(null == imageSocket)
			{
				code = 1;
				imageSocket= new ServerSocket(9096);
				code = 2;
				imageSocket.setSoTimeout(20);
			}

			code = 3;
			Socket socket=imageSocket.accept();
			code = 4;
			if(null == senderAddress)
			{
				code = 5;
				senderAddress = socket.getInetAddress();
			}
			code = 6;
			InputStream ips = socket.getInputStream();
			code = 7;
			image = BitmapFactory.decodeStream(ips);
			if(null != image)
			{
				Message msg = new Message();
				msg.what = 2;
				handler.sendMessage(msg);
				hasBegun = true;
			}
		} catch (Exception e) {
			// TODO Auto-generated catch block
			msg = "";
			switch(code)
			{
//			case 0:
//				break;
			case 1:
				msg = "Please force close in App manager and restart.";
				break;
//			case 2:
//				break;
			case 3:
				if(hasBegun)
				{
					//msg = "Cannot receive data now.";
				}
//				else
//				{
//					msg = "Ready.";
//				}
				break;
//			case 4:
//				break;
//			case 5:
//				break;
//			case 6:
//				break;
//			case 7:
//				break;
			default:
				break;
			}
			if(msg.length() > 0)
			{
				if(!reportedError)
				{
					Message msg1 = new Message();
					msg1.what = 1;
					handler.sendMessage(msg1);
					reportedError = true;
				}
			}
		}
	}
	
	@Override
	protected void onPause() {
		// TODO Auto-generated method stub
		super.onPause();
		shutdownImageSocket();
		runningOnBackground = true;
//		ActivityManager activityManager = (ActivityManager) this.getSystemService(Context.ACTIVITY_SERVICE);
////	    activityManager.killBackgroundProcesses(this.getPackageName());
//		try {
//			Method forceStopPackage = activityManager.getClass().getDeclaredMethod("forceStopPackage", String.class);
//			forceStopPackage.setAccessible(true);
//			forceStopPackage.invoke(activityManager, this.getPackageName());
//		} catch (Exception e) {
//			// TODO Auto-generated catch block
//		}
	}

//	@Override
//	protected void onNewIntent(Intent intent) {
//		// TODO Auto-generated method stub
//		super.onNewIntent(intent);
//		info += "onNewIntent";
//		showInfo();
//	}
//
//	@Override
//	protected void onRestart() {
//		// TODO Auto-generated method stub
//		super.onRestart();
//		info += "onRestart";
//		showInfo();
//	}
//
//	@Override
//	protected void onResume() {
//		// TODO Auto-generated method stub
//		super.onResume();
//		info += "onResume";
//		showInfo();
//	}
	
	private void showInfo()
	{
		if(!runningOnBackground)
		{
			Toast.makeText(this, msg, Toast.LENGTH_LONG).show();
		}
	}

	@Override
	protected void onDestroy() {
		// TODO Auto-generated method stub
		super.onDestroy();
		shutdownImageSocket();
	}
	
	private void shutdownImageSocket()
	{
		//info += "1_";
		if(null != imageSocket)
		{
			//info += "2_";
			//if(!imageSocket.isClosed())
			{
				try {
					//info += "3_";
					imageSocket.close();
					//info += "4_";
				} catch (Exception e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
					//info += "5_" + e.getMessage();
				}
			}
			imageSocket = null;
		}
	}

	private ImageView imageView1 = null;
	private static int cnt = 5;
	private DatagramSocket udpSocket;

	private Bitmap image = null;
	private Handler handler = new Handler() {
		@Override
		public void handleMessage(Message msg) {

			switch (msg.what) {
			case 1:
				showInfo();
				break;
			case 2:
				//imageView1.setScaleType(ScaleType.MATRIX);
				imageView1.setImageBitmap(image);
				//notifyRefreshDone();
				break;
			}
			super.handleMessage(msg);
		}
	};
	
	
	@Override
	public boolean onTouchEvent(MotionEvent event) {
		// TODO Auto-generated method stub
		//return super.onTouchEvent(event);
		return mGestureDetector.onTouchEvent(event);
	}

	@Override
	public boolean onDown(MotionEvent e) {
		// TODO Auto-generated method stub
		return false;
	}

	@Override
	public boolean onFling(MotionEvent e1, MotionEvent e2, float velocityX,
			float velocityY) {
		// TODO Auto-generated method stub
		return false;
	}

	@Override
	public void onLongPress(MotionEvent e) {
		// TODO Auto-generated method stub
		sendTouchEvent(RIGHT_CLICK, e);
	}

	@Override
	public boolean onScroll(MotionEvent e1, MotionEvent e2, float distanceX,
			float distanceY) {
		// TODO Auto-generated method stub
		return false;
	}

	@Override
	public void onShowPress(MotionEvent e) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public boolean onSingleTapUp(MotionEvent e) {
		// TODO Auto-generated method stub
		sendTouchEvent(CLICK, e);
		return false;
	}
}