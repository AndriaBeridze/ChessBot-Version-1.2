using System.Media;

namespace Chess.Scripts.Data
{
      internal class MoveSound
      {
            public SoundPlayer Move = new SoundPlayer("Sounds/Move.wav");
            public SoundPlayer Check = new SoundPlayer("Sounds/Check.wav");
            public SoundPlayer Capture = new SoundPlayer("Sounds/Capture.wav");
            public SoundPlayer Castle = new SoundPlayer("Sounds/Castle.wav");
            public SoundPlayer GameOver = new SoundPlayer("Sounds/GameOver.wav");
            public SoundPlayer IllegalMove = new SoundPlayer("Sounds/IllegalMove.wav");
      }
}
