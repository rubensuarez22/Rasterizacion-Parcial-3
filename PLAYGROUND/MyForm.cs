using System;
using System.Windows.Forms;

namespace PLAYGROUND
{
    public partial class MyForm : Form
    {
        Scene scene;
        Renderer renderer;
        Canvas canvas;

        public MyForm()
        {
            InitializeComponent();
        }

        private void Init()
        {
            canvas      = new Canvas(PCT_CANVAS);
            renderer    = new Renderer(canvas);
            scene       = new Scene();

            Mesh loadedMesh = ObjLoader.Load("Obj.obj");
            scene.AddModel(loadedMesh);
        }

        private void MyForm_SizeChanged(object sender, EventArgs e)
        {
            Init();
        }
                
        private void TIMER_Tick(object sender, EventArgs e)
        {
            renderer.RenderScene(scene);
          
        }
    }
}
