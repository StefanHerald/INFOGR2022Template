using System;
using System.Drawing;
using System.Threading;
using INFOGR2022Template;
using OpenTK;
using OpenTK.Graphics.OpenGL;

// The template provides you with a window which displays a 'linear frame buffer', i.e.
// a 1D array of pixels that represents the graphical contents of the window.

// Under the hood, this array is encapsulated in a 'Surface' object, and copied once per
// frame to an OpenGL texture, which is then used to texture 2 triangles that exactly
// cover the window. This is all handled automatically by the template code.

// Before drawing the two triangles, the template calls the Tick method in MyApplication,
// in which you are expected to modify the contents of the linear frame buffer.

// After (or instead of) rendering the triangles you can add your own OpenGL code.

// We will use both the pure pixel rendering as well as straight OpenGL code in the
// tutorial. After the tutorial you can throw away this template code, or modify it at
// will, or maybe it simply suits your needs.

namespace Template
{
	public class OpenTKApp : GameWindow
	{
		OpenTK.Input.KeyboardState previouskeys;
		static int screenID;            // unique integer identifier of the OpenGL texture
		internal static MyApplication app { get; private set; }       // instance of the application
		internal static Debug Debug { get; set; }
		static bool terminated = false; // application terminates gracefully when this is true
		protected override void OnLoad(EventArgs e)
		{
			// called during application initialization
			GL.ClearColor(0, 0, 0, 0);
			GL.Enable(EnableCap.Texture2D);
			GL.Disable(EnableCap.DepthTest);
			GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
			ClientSize = new Size(640, 400);
			app = new MyApplication();
			app.screen = new Surface(Width, Height);
			Sprite.target = app.screen;
			screenID = app.screen.GenTexture();
			app.Init();
			Debug = new Debug();
			Debug.debugScreen = new Surface(Width, Height);
			Debug.Init();
		}
		protected override void OnUnload(EventArgs e)
		{
			// called upon app close
			GL.DeleteTextures(1, ref screenID);
			Environment.Exit(0);      // bypass wait for key on CTRL-F5
		}
		protected override void OnResize(EventArgs e)
		{
			// called upon window resize. Note: does not change the size of the pixel buffer.
			GL.Viewport(0, 0, Width, Height);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 4.0);
		}
		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			// called once per frame; app logic
			var keyboard = OpenTK.Input.Keyboard.GetState();
			if (keyboard[OpenTK.Input.Key.Escape] && keyboard != previouskeys)
				terminated = true;
			if (keyboard[OpenTK.Input.Key.B] && keyboard != previouskeys)
				app.debugMode = !app.debugMode;
			previouskeys = keyboard;
			//Movement for the camera
			//Vertical (Y)
			if (keyboard[OpenTK.Input.Key.R] && keyboard != previouskeys)
				MyApplication.camera.position.Y -= .5f;
			if (keyboard[OpenTK.Input.Key.F] && keyboard != previouskeys)
				MyApplication.camera.position.Y += .5f;
			//Horizontal (X)
			if (keyboard[OpenTK.Input.Key.A] && keyboard != previouskeys)
				MyApplication.camera.position.X -= .5f;
			if (keyboard[OpenTK.Input.Key.D] && keyboard != previouskeys)
				MyApplication.camera.position.X += .5f;
			//Depth(Z)
			if (keyboard[OpenTK.Input.Key.W] && keyboard != previouskeys)
				MyApplication.camera.position.Z += .5f;
			if (keyboard[OpenTK.Input.Key.S] && keyboard != previouskeys)
				MyApplication.camera.position.Z -= .5f;
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			//added from Onload

			GL.ClearColor(0, 0, 0, 0);
			GL.Enable(EnableCap.Texture2D);
			GL.Disable(EnableCap.DepthTest);
			GL.Color3(1.0f, 1.0f, 1.0f);
			// called once per frame; render
			app.Tick();
			if (terminated)
			{
				Exit();
				return;
			}
			// convert MyApplication.screen to OpenGL texture
			GL.BindTexture(TextureTarget.Texture2D, screenID);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
						   app.screen.width, app.screen.height, 0,
						   OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
						   PixelType.UnsignedByte, app.screen.pixels
						 );
			// draw screen filling quad
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(-1.0f, -1.0f);
			GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(1.0f, -1.0f);
			GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(1.0f, 1.0f);
			GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(-1.0f, 1.0f);
			GL.End();
			// prepare for generic OpenGL rendering
			GL.Enable(EnableCap.DepthTest);
			GL.Disable(EnableCap.Texture2D);
			GL.Clear(ClearBufferMask.DepthBufferBit);
			// tell OpenTK we're done rendering
			SwapBuffers();
		}
		public static void Main(string[] args)
		{
			using (OpenTKApp app = new OpenTKApp()) { app.Run(30.0, 0.0); }
		}
	}
}