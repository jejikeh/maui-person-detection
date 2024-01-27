import base64
import io
import mediapipe as mp
from mediapipe.tasks import python
from mediapipe.tasks.python import vision
from mediapipe import solutions
from mediapipe.framework.formats import landmark_pb2
from PIL import Image
import cv2
import numpy as np

# Landmarker

landmarker_model_path = './model/pose_landmarker_full.task'
landmarker_base_landmarker_options = python.BaseOptions(
    model_asset_path=landmarker_model_path)
landmarker_options = vision.PoseLandmarkerOptions(
    base_options=landmarker_base_landmarker_options,
    output_segmentation_masks=True)
landmarker_detector = vision.PoseLandmarker.create_from_options(
    landmarker_options)


def landmarker(imageBase64):
    img = Image.open(io.BytesIO(base64.decodebytes(
        bytes(imageBase64, "utf-8")))).convert('RGB')
    image = mp.Image(image_format=mp.ImageFormat.SRGB, data=np.array(img))

    detection_result = landmarker_detector.detect(image)
    annotated_image = draw_landmarks_on_image(
        image.numpy_view(), detection_result)

    image = cv2.cvtColor(annotated_image, cv2.COLOR_RGB2BGR)
    _, buffer = cv2.imencode('.jpg', image)

    return base64.b64encode(buffer).decode('utf-8')


def draw_landmarks_on_image(rgb_image, detection_result):
    pose_landmarks_list = detection_result.pose_landmarks
    annotated_image = np.copy(rgb_image)

    for idx in range(len(pose_landmarks_list)):
        pose_landmarks = pose_landmarks_list[idx]
        pose_landmarks_proto = landmark_pb2.NormalizedLandmarkList()

        pose_landmarks_proto.landmark.extend([
            landmark_pb2.NormalizedLandmark(x=landmark.x, y=landmark.y, z=landmark.z) for landmark in pose_landmarks
        ])

        solutions.drawing_utils.draw_landmarks(
            annotated_image,
            pose_landmarks_proto,
            solutions.pose.POSE_CONNECTIONS,
            solutions.drawing_styles.get_default_pose_landmarks_style())
    return annotated_image


# Object Detection

MARGIN = 10  # pixels
ROW_SIZE = 10  # pixels
FONT_SIZE = 2
FONT_THICKNESS = 2
TEXT_COLOR = (255, 0, 0)  # red

object_detection_model_path = './model/efficientdet.tflite'
object_detection_base_options = python.BaseOptions(
    model_asset_path=object_detection_model_path)
object_detection_options = vision.ObjectDetectorOptions(base_options=object_detection_base_options,
                                                        score_threshold=0.5)
object_detector = vision.ObjectDetector.create_from_options(
    object_detection_options)


def object_detection(imageBase64):
    img = Image.open(io.BytesIO(base64.decodebytes(
        bytes(imageBase64, "utf-8")))).convert('RGB')
    image = mp.Image(image_format=mp.ImageFormat.SRGB, data=np.array(img))

    detection_result = object_detector.detect(image)
    annotated_image = visualize(image.numpy_view(), detection_result)

    image = cv2.cvtColor(annotated_image, cv2.COLOR_RGB2BGR)
    _, buffer = cv2.imencode('.jpg', image)

    return base64.b64encode(buffer).decode('utf-8')


def visualize(img, detection_result) -> np.ndarray:
    # copy image
    image = np.copy(img)
    image.setflags(write=1)
    for detection in detection_result.detections:
        bbox = detection.bounding_box
        start_point = bbox.origin_x, bbox.origin_y
        end_point = bbox.origin_x + bbox.width, bbox.origin_y + bbox.height
        cv2.rectangle(image, start_point, end_point, TEXT_COLOR, 3)

        # Draw label and score
        category = detection.categories[0]
        category_name = category.category_name
        probability = round(category.score, 2)
        result_text = category_name + ' (' + str(probability) + ')'
        text_location = (MARGIN + bbox.origin_x,
                         MARGIN + ROW_SIZE + bbox.origin_y)
        cv2.putText(image, result_text, text_location, cv2.FONT_HERSHEY_PLAIN,
                    FONT_SIZE, TEXT_COLOR, FONT_THICKNESS)

    return image
