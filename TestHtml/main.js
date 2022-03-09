const link = 'http://localhost:5285/GiftCard/';

const ul = document.getElementById('giftCards');
const list = document.createDocumentFragment();
fetch(link,
{
    method: 'Get',
    //mode: 'no-cors',
    // headers:{
    //     'Content-type': 'application/json'
    // }

})
.then(resp => {
    if(!resp.ok)
    {
        throw new Error('Network response was not ok');
    }
    return resp.json();

})
.then(data => {
    let giftCards = data;
    giftCards.map(giftcard =>
    {
        let li = document.createElement('li');
        let number = document.createElement('span');
        number.innerHTML = giftcard.number;


        li.appendChild(number);
        list.appendChild(li);

    });
    ul.appendChild(list);
})
.catch(reason => {
    const errorRep = {
        message: 'Something went wrong',
        code: '55555'
    }
    
    console.error('There has been a problem',reason);
    json = JSON.stringify(errorRep);
    console.log(json);
    return json;
});

