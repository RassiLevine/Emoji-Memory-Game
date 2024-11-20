using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmojiMemoryGameSystem
{
    public class EmojiCard
    {
        public string emoji { get; set; }
        
        public EmojiCard(string emojistring)
        {
            emoji = emojistring;
        }
    }
}
