from ultralytics import YOLO

# Load a model
model = YOLO('yolov8n-seg.pt')  # load an official model

# Export the quantized model
model.export(format='onnx', half=True, simplify=True)

# Export the un-quantized model
# model.export(format='onnx')
