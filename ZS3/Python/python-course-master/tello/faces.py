#!/usr/bin/env python3

import tellopy
import cv2
import numpy as np
import sys
import av
import time

def decode_frames(drone, ttl):
    retry = 3
    container = None
    while container is None and 0 < retry:
        retry -= 1
        try:
            container = av.open(drone.get_video_stream())
        except av.AVError as ave:
            print(ave)
            print('retry...')

    # skip first 300 frames
    frame_skip = 300
    i = 0
    while True:
        for frame in container.decode(video=0):
            if 0 < frame_skip:
                frame_skip = frame_skip - 1
                continue
            start_time = time.time()
            image = cv2.cvtColor(np.array(frame.to_image()), cv2.COLOR_RGB2BGR)
            if frame.time_base < 1.0/60:
                time_base = 1.0/60
            else:
                time_base = frame.time_base
            frame_skip = int((time.time() - start_time)/time_base)
            if i % 5 == 0:
                yield image
            i += 1

def _main():
    drone = tellopy.Tello()
    drone.connect()
    drone.wait_for_connection(60.0)
    dry_run = False
    speed = 12
    tol = 50

    # Create the haar cascade
    cascPath = "haarcascade_frontalface_default.xml"
    faceCascade = cv2.CascadeClassifier(cascPath)

    frames = decode_frames(drone, 5)
    w, h = 1000, 720
    rx = w // 2
    ry = h // 2

    print("Before dry_run")
    if not dry_run:
        time.sleep(2)
        drone.takeoff()
        time.sleep(2)

    for frame in frames:
        gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)

        faces = faceCascade.detectMultiScale(
            gray,
            scaleFactor=1.1,
            minNeighbors=5,
            minSize=(40, 40),
            flags = cv2.CASCADE_SCALE_IMAGE
        )

        cv2.rectangle(frame, (rx - tol, ry - tol), (rx + tol, ry + tol), (255, 0, 0), 2)

        if len(faces):
            face = max(faces, key = lambda f: f[2] * f[3])
            x, y, w, h = face
            cx, cy = x + w // 2, y + h // 2
            cv2.rectangle(frame, (x, y), (x + w, y + h), (0, 255, 0), 2)
            cv2.rectangle(frame, (cx - 2, cy - 2), (cx + 2, cy + 2), (0, 0, 255), 2)
            dx, dy = rx - cx, ry - cy
            up = dy > tol
            down = dy < -tol
            left = dx > tol
            right = dx < -tol
            print("X", "left" if left else "right" if right else "+")
            print("Y", "up" if up else "down" if down else "+")

            if not dry_run:
                if up:
                    drone.up(speed)
                if down:
                    drone.down(speed)
                if left:
                    drone.counter_clockwise(speed)
                if right:
                    drone.clockwise(speed)
        elif not dry_run:
            drone.clockwise(speed // 2)

        cv2.imshow("drone", frame)
        key = cv2.waitKey(1) & 0xFF
        if key == ord('q'):
            break
        elif key == ord('+'):
            speed += 2
        elif key == ord('-'):
            speed -= 2
        elif key == ord('w'):
            drone.forward(20)
        elif key == ord('s'):
            drone.backward(20)
        elif key == ord('a'):
            drone.counter_clockwise(20)
        elif key == ord('d'):
            drone.clockwise(20)

    if not dry_run:
        drone.land()

if __name__ == "__main__":
    _main()
