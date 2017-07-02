// programmed by Adrian Magdina in 2013
// in this file is the implementation of viewbehind for "About CA Explorer" dialog
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace CAExplorerNamespace
{
    /// <summary>
    /// Interaction logic for AboutCAExplorer.xaml
    /// </summary>
    public partial class AboutCAExplorer : UserControl
    {
        #region Constructors

        public AboutCAExplorer()
        {
            InitializeComponent();

            //creating new timer, that will cause the text rotate during Tick
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

            //calling method for rotation during Tick
            dispatcherTimer.Tick += new EventHandler(Rotate3DGeometry);
        }

        #endregion

        #region Methods

        private void viewport3DAboutProgram_Loaded(object sender, RoutedEventArgs e)
        {
            myRotationAngle = 0;

            // Declare scene objects.
            myModel3DGroup = new Model3DGroup();
            myModelVisual3D = new ModelVisual3D();

            //Tick should occur every 20ms
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            dispatcherTimer.Start();
        }

        private void Rotate3DGeometry(object sender, EventArgs e)
        {
            //Delaying the rotation during start, and after every full revolution
            if (myRotationAngle == 1 && myDelayingOfRotation < myDelayingOfRotationConstant)
            {
                myDelayingOfRotation++;
                return;
            }

            if (myDelayingOfRotation >= myDelayingOfRotationConstant)
            {
                myDelayingOfRotation = 0;
            }

            //rotating 1 degrees every Tick
            myRotationAngle += 1;

            if (myRotationAngle >= 360)
            {
                myRotationAngle = 0;
            }

            this.viewport3DAboutProgram.Children.Clear();
            myModel3DGroup.Children.Clear();

            //creating Perspective camera
            PerspectiveCamera aCamera = CreateCamera();

            // Asign the camera to the viewport
            this.viewport3DAboutProgram.Camera = aCamera;

            // Define the lights cast in the scene. Without light, the 3D object cannot 
            // be seen. Also, the direction of the lights affect shadowing. Note: to 
            // illuminate an object from additional directions, create additional lights.
            DirectionalLight aDirectionalLight = CreateDirectionalLight();

            myModel3DGroup.Children.Add(aDirectionalLight);

            // Create a collection of vertex positions for the MeshGeometry3D. 
            Point3DCollection myPositionCollection = new Point3DCollection();
            myPositionCollection.Add(new Point3D(-0.5, 0.2, 0.0));
            myPositionCollection.Add(new Point3D(0.5, 0.2, 0.0));
            myPositionCollection.Add(new Point3D(0.5, 0.4, 0.0));
            myPositionCollection.Add(new Point3D(0.5, 0.4, 0.0));
            myPositionCollection.Add(new Point3D(-0.5, 0.4, 0.0));
            myPositionCollection.Add(new Point3D(-0.5, 0.2, 0.0));

            // Create a collection of texture coordinates for the MeshGeometry3D.
            PointCollection myTextureCoordinatesCollection = new PointCollection();
            myTextureCoordinatesCollection.Add(new Point(0, 1));
            myTextureCoordinatesCollection.Add(new Point(1, 1));
            myTextureCoordinatesCollection.Add(new Point(1, 0));
            myTextureCoordinatesCollection.Add(new Point(1, 0));
            myTextureCoordinatesCollection.Add(new Point(0, 0));
            myTextureCoordinatesCollection.Add(new Point(0, 1));

            GeometryModel3D aGeometryModelFrontAppName = CreateGeometryModel3D(myPositionCollection, myTextureCoordinatesCollection, 1.0, "CA Explorer");

            // Create a collection of vertex positions for the MeshGeometry3D. 
            myPositionCollection = new Point3DCollection();
            myPositionCollection.Add(new Point3D(-1.0, 0.0, 0.0));
            myPositionCollection.Add(new Point3D(1.0, 0.0, 0.0));
            myPositionCollection.Add(new Point3D(1.0, 0.2, 0.0));
            myPositionCollection.Add(new Point3D(1.0, 0.2, 0.0));
            myPositionCollection.Add(new Point3D(-1.0, 0.2, 0.0));
            myPositionCollection.Add(new Point3D(-1.0, 0.0, 0.0));

            // Create a collection of texture coordinates for the MeshGeometry3D.
            myTextureCoordinatesCollection = new PointCollection();
            myTextureCoordinatesCollection.Add(new Point(0, 1));
            myTextureCoordinatesCollection.Add(new Point(1, 1));
            myTextureCoordinatesCollection.Add(new Point(1, 0));
            myTextureCoordinatesCollection.Add(new Point(1, 0));
            myTextureCoordinatesCollection.Add(new Point(0, 0));
            myTextureCoordinatesCollection.Add(new Point(0, 1));

            GeometryModel3D aGeometryModelFrontMyName = CreateGeometryModel3D(myPositionCollection, myTextureCoordinatesCollection, 1.0, "Programmed by Adrian Magdina");

            // Create a collection of vertex positions for the MeshGeometry3D. 
            myPositionCollection = new Point3DCollection();
            myPositionCollection.Add(new Point3D(-0.3, -0.2, 0.0));
            myPositionCollection.Add(new Point3D(0.3, -0.2, 0.0));
            myPositionCollection.Add(new Point3D(0.3, 0.0, 0.0));
            myPositionCollection.Add(new Point3D(0.3, 0.0, 0.0));
            myPositionCollection.Add(new Point3D(-0.3, 0.0, 0.0));
            myPositionCollection.Add(new Point3D(-0.3, -0.2, 0.0));

            // Create a collection of texture coordinates for the MeshGeometry3D.
            myTextureCoordinatesCollection = new PointCollection();
            myTextureCoordinatesCollection.Add(new Point(0, 1));
            myTextureCoordinatesCollection.Add(new Point(1, 1));
            myTextureCoordinatesCollection.Add(new Point(1, 0));
            myTextureCoordinatesCollection.Add(new Point(1, 0));
            myTextureCoordinatesCollection.Add(new Point(0, 0));
            myTextureCoordinatesCollection.Add(new Point(0, 1));

            GeometryModel3D aGeometryModelFrontYear = CreateGeometryModel3D(myPositionCollection, myTextureCoordinatesCollection, 1.0, "in 2013");

            // Create a collection of vertex positions for the MeshGeometry3D. 
            myPositionCollection = new Point3DCollection();
            myPositionCollection.Add(new Point3D(-1.0, -0.4, 0.0));
            myPositionCollection.Add(new Point3D(1.0, -0.4, 0.0));
            myPositionCollection.Add(new Point3D(1.0, -0.2, 0.0));
            myPositionCollection.Add(new Point3D(1.0, -0.2, 0.0));
            myPositionCollection.Add(new Point3D(-1.0, -0.2, 0.0));
            myPositionCollection.Add(new Point3D(-1.0, -0.4, 0.0));

            // Create a collection of texture coordinates for the MeshGeometry3D.
            myTextureCoordinatesCollection = new PointCollection();
            myTextureCoordinatesCollection.Add(new Point(0, 1));
            myTextureCoordinatesCollection.Add(new Point(1, 1));
            myTextureCoordinatesCollection.Add(new Point(1, 0));
            myTextureCoordinatesCollection.Add(new Point(1, 0));
            myTextureCoordinatesCollection.Add(new Point(0, 0));
            myTextureCoordinatesCollection.Add(new Point(0, 1));

            GeometryModel3D aGeometryModelFrontEMail = CreateGeometryModel3D(myPositionCollection, myTextureCoordinatesCollection, 1.0, "adrian.magdina@yahoo.de");

            // Create a collection of vertex positions for the MeshGeometry3D. 
            myPositionCollection = new Point3DCollection();

            myPositionCollection.Add(new Point3D(0.5, 0.4, 0.0));
            myPositionCollection.Add(new Point3D(0.5, 0.2, 0.0));
            myPositionCollection.Add(new Point3D(-0.5, 0.2, 0.0));
            myPositionCollection.Add(new Point3D(-0.5, 0.2, 0.0));
            myPositionCollection.Add(new Point3D(-0.5, 0.4, 0.0));
            myPositionCollection.Add(new Point3D(0.5, 0.4, 0.0));

            // Create a collection of texture coordinates for the MeshGeometry3D.
            myTextureCoordinatesCollection = new PointCollection();
            myTextureCoordinatesCollection.Add(new Point(1, 0));
            myTextureCoordinatesCollection.Add(new Point(1, 1));
            myTextureCoordinatesCollection.Add(new Point(0, 1));
            myTextureCoordinatesCollection.Add(new Point(0, 1));
            myTextureCoordinatesCollection.Add(new Point(0, 0));
            myTextureCoordinatesCollection.Add(new Point(1, 0));

            GeometryModel3D aGeometryModelBackAppName = CreateGeometryModel3D(myPositionCollection, myTextureCoordinatesCollection, 1.0, "CA Explorer");

            // Create a collection of vertex positions for the MeshGeometry3D. 
            myPositionCollection = new Point3DCollection();

            myPositionCollection.Add(new Point3D(1.0, 0.2, 0.0));
            myPositionCollection.Add(new Point3D(1.0, 0.0, 0.0));
            myPositionCollection.Add(new Point3D(-1.0, 0.0, 0.0));
            myPositionCollection.Add(new Point3D(-1.0, 0.0, 0.0));
            myPositionCollection.Add(new Point3D(-1.0, 0.2, 0.0));
            myPositionCollection.Add(new Point3D(1.0, 0.2, 0.0));

            // Create a collection of texture coordinates for the MeshGeometry3D.
            myTextureCoordinatesCollection = new PointCollection();
            myTextureCoordinatesCollection.Add(new Point(1, 0));
            myTextureCoordinatesCollection.Add(new Point(1, 1));
            myTextureCoordinatesCollection.Add(new Point(0, 1));
            myTextureCoordinatesCollection.Add(new Point(0, 1));
            myTextureCoordinatesCollection.Add(new Point(0, 0));
            myTextureCoordinatesCollection.Add(new Point(1, 0));

            GeometryModel3D aGeometryModelBackMyName = CreateGeometryModel3D(myPositionCollection, myTextureCoordinatesCollection, 1.0, "Programmed by Adrian Magdina");

            // Create a collection of vertex positions for the MeshGeometry3D. 
            myPositionCollection = new Point3DCollection();

            myPositionCollection.Add(new Point3D(0.3, 0.0, 0.0));
            myPositionCollection.Add(new Point3D(0.3, -0.2, 0.0));
            myPositionCollection.Add(new Point3D(-0.3, -0.2, 0.0));
            myPositionCollection.Add(new Point3D(-0.3, -0.2, 0.0));
            myPositionCollection.Add(new Point3D(-0.3, 0.0, 0.0));
            myPositionCollection.Add(new Point3D(0.3, 0.0, 0.0));

            // Create a collection of texture coordinates for the MeshGeometry3D.
            myTextureCoordinatesCollection = new PointCollection();
            myTextureCoordinatesCollection.Add(new Point(1, 0));
            myTextureCoordinatesCollection.Add(new Point(1, 1));
            myTextureCoordinatesCollection.Add(new Point(0, 1));
            myTextureCoordinatesCollection.Add(new Point(0, 1));
            myTextureCoordinatesCollection.Add(new Point(0, 0));
            myTextureCoordinatesCollection.Add(new Point(1, 0));

            GeometryModel3D aGeometryModelBackYear = CreateGeometryModel3D(myPositionCollection, myTextureCoordinatesCollection, 1.0, "in 2013");

            // Create a collection of vertex positions for the MeshGeometry3D. 
            myPositionCollection = new Point3DCollection();

            myPositionCollection.Add(new Point3D(1.0, -0.2, 0.0));
            myPositionCollection.Add(new Point3D(1.0, -0.4, 0.0));
            myPositionCollection.Add(new Point3D(-1.0, -0.4, 0.0));
            myPositionCollection.Add(new Point3D(-1.0, -0.4, 0.0));
            myPositionCollection.Add(new Point3D(-1.0, -0.2, 0.0));
            myPositionCollection.Add(new Point3D(1.0, -0.2, 0.0));

            // Create a collection of texture coordinates for the MeshGeometry3D.
            myTextureCoordinatesCollection = new PointCollection();
            myTextureCoordinatesCollection.Add(new Point(1, 0));
            myTextureCoordinatesCollection.Add(new Point(1, 1));
            myTextureCoordinatesCollection.Add(new Point(0, 1));
            myTextureCoordinatesCollection.Add(new Point(0, 1));
            myTextureCoordinatesCollection.Add(new Point(0, 0));
            myTextureCoordinatesCollection.Add(new Point(1, 0));

            GeometryModel3D aGeometryModelBackEMail = CreateGeometryModel3D(myPositionCollection, myTextureCoordinatesCollection, 1.0, "adrian.magdina@yahoo.de");

            // Apply a transform to the object. In this sample, a rotation transform is applied,  
            // rendering the 3D object rotated.
            RotateTransform3D myRotateTransform3D = new RotateTransform3D();
            AxisAngleRotation3D myAxisAngleRotation3d = new AxisAngleRotation3D();
            myAxisAngleRotation3d.Axis = new Vector3D(0, 3, 0);
            myAxisAngleRotation3d.Angle = myRotationAngle;
            myRotateTransform3D.Rotation = myAxisAngleRotation3d;
            aGeometryModelFrontAppName.Transform = myRotateTransform3D;
            aGeometryModelFrontMyName.Transform = myRotateTransform3D;
            aGeometryModelFrontYear.Transform = myRotateTransform3D;
            aGeometryModelFrontEMail.Transform = myRotateTransform3D;
            aGeometryModelBackAppName.Transform = myRotateTransform3D;
            aGeometryModelBackMyName.Transform = myRotateTransform3D;
            aGeometryModelBackYear.Transform = myRotateTransform3D;
            aGeometryModelBackEMail.Transform = myRotateTransform3D;

            // Add the geometry model to the model group.
            myModel3DGroup.Children.Add(aGeometryModelFrontAppName);
            myModel3DGroup.Children.Add(aGeometryModelFrontMyName);
            myModel3DGroup.Children.Add(aGeometryModelFrontYear);
            myModel3DGroup.Children.Add(aGeometryModelFrontEMail);
            myModel3DGroup.Children.Add(aGeometryModelBackAppName);
            myModel3DGroup.Children.Add(aGeometryModelBackMyName);
            myModel3DGroup.Children.Add(aGeometryModelBackYear);
            myModel3DGroup.Children.Add(aGeometryModelBackEMail);

            // Add the group of models to the ModelVisual3d.
            myModelVisual3D.Content = myModel3DGroup;

            this.viewport3DAboutProgram.Children.Add(myModelVisual3D);
        }

        private GeometryModel3D CreateGeometryModel3D(Point3DCollection meshPointListIn, PointCollection textureCoordinatesIn, double normalZOrientationIn, string captionIn)
        {
            // The geometry specifes the shape of the 3D plane. In this sample, a flat sheet
            // is created.
            MeshGeometry3D aMeshGeometry3D = new MeshGeometry3D();

            // Create a collection of normal vectors for the MeshGeometry3D.
            Vector3DCollection myNormalCollection = new Vector3DCollection();
            myNormalCollection.Add(new Vector3D(0, 0, normalZOrientationIn));
            myNormalCollection.Add(new Vector3D(0, 0, normalZOrientationIn));
            myNormalCollection.Add(new Vector3D(0, 0, normalZOrientationIn));
            myNormalCollection.Add(new Vector3D(0, 0, normalZOrientationIn));
            myNormalCollection.Add(new Vector3D(0, 0, normalZOrientationIn));
            myNormalCollection.Add(new Vector3D(0, 0, normalZOrientationIn));

            aMeshGeometry3D.Normals = myNormalCollection;

            aMeshGeometry3D.Positions = meshPointListIn;

            aMeshGeometry3D.TextureCoordinates = textureCoordinatesIn;

            // Declare scene objects.
            GeometryModel3D aGeometryModel = new GeometryModel3D();

            // Apply the mesh to the geometry model.
            aGeometryModel.Geometry = aMeshGeometry3D;

            // Create a horizontal linear gradient with four stops.   
            VisualBrush aVisualBrush = new VisualBrush();
            Label aLabel = new Label();
            aLabel.Foreground = new SolidColorBrush(Colors.MidnightBlue);
            aLabel.Content = captionIn;
            aVisualBrush.Visual = aLabel;

            // Define material and apply to the mesh geometries.
            MaterialGroup aMaterial = CreateMaterial(aVisualBrush);
            aGeometryModel.Material = aMaterial;

            return aGeometryModel;
        }

        private PerspectiveCamera CreateCamera()
        {
            // Defines the camera used to view the 3D object. In order to view the 3D object,
            // the camera must be positioned and pointed such that the object is within view 
            // of the camera.
            PerspectiveCamera aPerspectiveCamera = new PerspectiveCamera();

            // Specify where in the 3D scene the camera is.
            aPerspectiveCamera.Position = new Point3D(0, 0, 2.0);

            // Specify the direction that the camera is pointing.
            aPerspectiveCamera.LookDirection = new Vector3D(0, 0, -1.0);

            // Define camera's horizontal field of view in degrees.
            aPerspectiveCamera.FieldOfView = 60.0;

            return aPerspectiveCamera;
        }

        private MaterialGroup CreateMaterial(VisualBrush visualBrushIn)
        {
            MaterialGroup aMaterialGroup = new MaterialGroup();

            Material aDiffuseMaterial = new DiffuseMaterial(visualBrushIn);

            aMaterialGroup.Children.Add(aDiffuseMaterial);

            return aMaterialGroup;
        }

        private DirectionalLight CreateDirectionalLight()
        {
            DirectionalLight aDirectionalLight = new DirectionalLight();
            aDirectionalLight.Color = Colors.LightYellow;
            aDirectionalLight.Direction = new Vector3D(-0.8, -0.2, -1.0);

            return aDirectionalLight;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            ContentPresenter aCP = this.TemplatedParent as ContentPresenter;
            if (aCP != null)
            {
                Grid aGrid = aCP.Parent as Grid;

                if (aGrid != null)
                {
                    Window aWindow = aGrid.Parent as Window;
                    if (aWindow != null)
                    {
                        aWindow.DialogResult = true;
                        aWindow.Close();
                    }
                }
            }
        }

        private void viewport3DAboutProgram_Unloaded(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
        }

        #endregion

        #region Members

        private DispatcherTimer dispatcherTimer = null;

        private int myRotationAngle = 0;
        private int myDelayingOfRotation = 0;
        private const int myDelayingOfRotationConstant = 30;

        ModelVisual3D myModelVisual3D = null;
        Model3DGroup myModel3DGroup = null;

        #endregion
    }
}
