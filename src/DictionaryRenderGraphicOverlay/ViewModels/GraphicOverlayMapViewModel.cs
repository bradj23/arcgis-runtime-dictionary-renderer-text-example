using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using DictionaryRenderGraphicOverlay.Views;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Mapping.Labeling;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.UI.Controls;

namespace DictionaryRenderGraphicOverlay.ViewModels
{
    public class GraphicOverlayMapViewModel : ViewAware
    {
        private MapView _mapView;
        private MapPoint _mapPoint1 = new(-105.0275148018931, 40.56930328177301, SpatialReferences.Wgs84);
        private MapPoint _mapPoint2 = new(-105.05026726718022, 40.570070204951904, SpatialReferences.Wgs84);
        private MapPoint _mapPoint3 = new(-105.0275148018931, 40.56930328177301, SpatialReferences.Wgs84);
        private GraphicsOverlay _indicatorGraphicsOverlay;
        private GraphicsOverlay _dictionaryRendererOverlay;

     
        public GraphicOverlayMapViewModel()
        {

        }

        public void NavigateToViewPoint1()
        {
            _mapView.SetViewpointCenterAsync(_mapPoint1, 6000);
        }

        public void NavigateToViewPoint2()
        {
            _mapView.SetViewpointCenterAsync(_mapPoint2, 8888);

        }

        public void AddExtentOutline()
        {
            var visibleArea = _mapView.VisibleArea;

            var extentSymbol =
                new SimpleFillSymbol(SimpleFillSymbolStyle.Null, Color.Transparent,
                    new SimpleLineSymbol(SimpleLineSymbolStyle.Solid, Color.PaleVioletRed, 2));

            var graphic = new Graphic(visibleArea, extentSymbol);
            _indicatorGraphicsOverlay.Graphics.Clear();
            _indicatorGraphicsOverlay.Graphics.Add(graphic);

        }

        public void NavigateToViewPoint3()
        {
            _mapView.SetViewpointCenterAsync(_mapPoint3, 100);
        }

        protected override void OnViewLoaded(object view)
        {
            if (view is GraphicOverlayMapView graphicOverlayMapView)
            {
                _mapView = graphicOverlayMapView.MapView;

                _mapView.Map.ReferenceScale = 4000;

                _indicatorGraphicsOverlay = new();

                _mapView.GraphicsOverlays.Add(_indicatorGraphicsOverlay);

                
            }

            base.OnViewLoaded(view);

            InitializeDictionaryRenderer().ContinueWith((task) =>
            {
                if (task.IsFaulted)
                {
                    // todo some sort of error handling
                }
            });
        }

        private async Task InitializeDictionaryRenderer()
        {
            _dictionaryRendererOverlay = new()
            {
                ScaleSymbols = true,
                LabelsEnabled = true
            };
            _mapView.GraphicsOverlays.Add(_dictionaryRendererOverlay);

            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var stylxPath = Path.Combine(baseDir, @"Stylx\ExampleStyle.stylx");

            var exampleStyle = await DictionarySymbolStyle.CreateFromFileAsync(stylxPath);
            _dictionaryRendererOverlay.Renderer = new DictionaryRenderer(exampleStyle);
            var attributes = new Dictionary<string, object>
            {
                { "Name", "A Shorter label" },
                { "LabelText", "Some new text"},
                { "Show", "true"}
            };
            var graphic = new Graphic(_mapPoint1, attributes);

            var exp = new ArcadeLabelExpression(@"$feature.LABELTEXT + ' Always show'" );
            var textSymbol = new TextSymbol();
            var name = @"* NEW LABEL *";
            var label = new LabelDefinition(exp, textSymbol);
            _dictionaryRendererOverlay.LabelDefinitions.Add(label);
            _dictionaryRendererOverlay.Graphics.Add(graphic);
            await _mapView.SetViewpointCenterAsync(_mapPoint1, 6000);

        }
    }
}