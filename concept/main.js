let earth, air;

let gravity = 0.7;
let friction = 0.8;
let accel = 0.6;
let maxSpeed = 4;

let platforms = [];

let earthAnchor = null;
let airDashCooldown = 0;

let earthLever = false;
let airLever = false;

let barrierOpen = false;

let airBarrier;
let exitZone;

function setup() {
  createCanvas(500, 400);

  earth = createPlayer(40, 300, "earth");
  air = createPlayer(60, 300, "air");

  exitZone = { x: 430, y: 300, w: 50, h: 50 };

  platforms = [
    { x: 0, y: 350, w: 500, h: 50 },
    { x: 60, y: 300, w: 80, h: 10 },
    { x: 160, y: 270, w: 80, h: 10 },
    { x: 260, y: 240, w: 80, h: 10 },
    { x: 380, y: 200, w: 80, h: 10 }
  ];

  earthLeverObj = { x: 420, y: 170, w: 20, h: 20 };
  airLeverObj = { x: 120, y: 80, w: 20, h: 20 };

  airBarrier = { x: 0, y: 120, w: 300, h: 20, active: true };
}

function draw() {
  background(30);
  updateGame();
}

function updateGame() {
  updateCooldowns();

  updatePlayer(earth);
  updatePlayer(air);

  updateEarthAnchor();

  handleLevers();

  resolveBarriers();

  drawWorld();

  if (checkWin()) {
    noLoop();
    fill("white");
    textAlign(CENTER);
    textSize(32);
    text("YOU WIN", width / 2, 120);
  }
}

function createPlayer(x, y, type) {
  return { x, y, w: 28, h: 28, vx: 0, vy: 0, onGround: false, type };
}

function updatePlayer(p) {
  if (p.type === "earth") {
    if (keyIsDown(65)) p.vx -= accel;
    if (keyIsDown(68)) p.vx += accel;
  } else {
    if (keyIsDown(LEFT_ARROW)) p.vx -= accel;
    if (keyIsDown(RIGHT_ARROW)) p.vx += accel;
  }

  p.vx = constrain(p.vx, -maxSpeed, maxSpeed);
  if (p.onGround) p.vx *= friction;

  p.vy += gravity;

  p.x += p.vx;
  p.y += p.vy;

  p.onGround = false;

  for (let plat of platforms) resolvePlatform(p, plat);
  if (earthAnchor) resolvePlatform(p, earthAnchor);
}

function keyPressed() {
  if ((key === 'w' || key === 'W') && earth.onGround) earth.vy = -10;
  if (keyCode === UP_ARROW && air.onGround) air.vy = -13;

  if (key === 'e' || key === 'E') {
    if (earth.onGround) {
      earthAnchor = {
        x: earth.x - 20,
        y: earth.y + earth.h,
        w: 80,
        h: 10,
        timer: 180
      };
    }
  }

  if (key === 'x' || key === 'X') {
    if (!air.onGround && airDashCooldown <= 0) {
      air.vx += (keyIsDown(LEFT_ARROW) ? -8 : 8);
      air.vy *= 0.5;
      airDashCooldown = 90;
    }
  }
}

function handleLevers() {
  if (collideRect(earth, earthLeverObj) && keyIsDown(69)) {
    earthLever = true;
  }

  if (collideRect(air, airLeverObj) && keyIsDown(SHIFT)) {
    airLever = true;
  }
}

function resolveBarriers() {
  if (earthLever) {
    airBarrier.active = false;
  }
}

function updateEarthAnchor() {
  if (!earthAnchor) return;

  earthAnchor.timer--;
  if (earthAnchor.timer <= 0) earthAnchor = null;
}

function updateCooldowns() {
  if (airDashCooldown > 0) airDashCooldown--;
}

function resolvePlatform(p, plat) {
  if (
    p.x < plat.x + plat.w &&
    p.x + p.w > plat.x &&
    p.y < plat.y + plat.h &&
    p.y + p.h > plat.y &&
    p.vy > 0
  ) {
    p.y = plat.y - p.h;
    p.vy = 0;
    p.onGround = true;
  }
}

function checkWin() {
  return earthLever && airLever && collideRect(earth, exitZone) && collideRect(air, exitZone);
}

function drawWorld() {
  fill(90);
  for (let p of platforms) rect(p.x, p.y, p.w, p.h);

  if (earthAnchor) {
    fill("sienna");
    rect(earthAnchor.x, earthAnchor.y, earthAnchor.w, earthAnchor.h);
  }

  if (airBarrier.active) {
    fill("red");
    rect(airBarrier.x, airBarrier.y, airBarrier.w, airBarrier.h);
  }

  fill(earthLever ? "lime" : "brown");
  rect(earthLeverObj.x, earthLeverObj.y, earthLeverObj.w, earthLeverObj.h);

  fill(airLever ? "cyan" : "blue");
  rect(airLeverObj.x, airLeverObj.y, airLeverObj.w, airLeverObj.h);

  fill(earthLever && airLever ? "yellow" : "darkgray");
  rect(exitZone.x, exitZone.y, exitZone.w, exitZone.h);

  fill("green");
  rect(earth.x, earth.y, earth.w, earth.h);

  fill("lightblue");
  rect(air.x, air.y, air.w, air.h);
}

function collideRect(a, b) {
  return (
    a.x < b.x + b.w &&
    a.x + a.w > b.x &&
    a.y < b.y + b.h &&
    a.y + a.h > b.y
  );
}
