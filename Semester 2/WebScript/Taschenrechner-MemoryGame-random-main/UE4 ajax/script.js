$(document).ready(() => {
  const [input, list, message, addButton, hideButton] = ['#input', 'ol', '#message', '#add', '#hide'].map(q => $(q));

  const addItem = text => {
    if (text) {
      const newItem = $('<li>').text(text).append('<button>x</button><button>↑</button><button>↓</button>');
      newItem.hide().appendTo(list).fadeIn();
      input.val('');
    }
  };

  list.on('click', 'button', function () {
    const parent = $(this).parent();
    $(this).index() === 0 ? parent.fadeOut(() => parent.remove()) && message.fadeIn().delay(500).fadeOut() : $(this).index() === 1 ? parent.insertBefore(parent.prev()) : parent.insertAfter(parent.next());
  });

  addButton.on('click', () => addItem(input.val()));
  hideButton.on('click', () => list.slideToggle());
  message.hide();
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  $.ajax({ url: "storyofmylife.php", type: "GET", dataType: "json", success: data => data.forEach(item => addItem(item)) });   // neu 
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
});
