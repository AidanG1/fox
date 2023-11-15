import math

coyote_and_jump_buffer = 0.1
jump_multiplier = 400

fps_velocity_list = []

for fps in range(1, 100):
    time_per_frame = 1 / fps

    # time_per_frame * x > 0.1, find min x
    coyote_frames = math.ceil(coyote_and_jump_buffer / time_per_frame)

    jump_velocity = jump_multiplier * time_per_frame * coyote_frames

    print(f"FPS: {fps}, Coyote Frames: {coyote_frames}, Jump Velocity: {jump_velocity}")

    fps_velocity_list.append((fps, jump_velocity))

# graph with matplotlib

import matplotlib.pyplot as plt

x, y = zip(*fps_velocity_list)
plt.plot(x, y)
plt.xlabel("FPS")
plt.ylabel("Jump Velocity")
