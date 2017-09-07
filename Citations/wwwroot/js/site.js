//модель для цитаты
function Citation(id, text, author, date, category = null) {
    this.id = id;
    this.text = text;
    this.author = author;
    var newDate = new Date(date);
    this.date = newDate.getDate() + '-' + newDate.getMonth()+'-'+newDate.getFullYear();
    this.category = category;
}

//привязка объекта к изменениям списка цитат
var ViewModel = function () {
    this.citations = ko.observableArray();
}

viewModel = new ViewModel();

//получение списка цитат
viewModel.refresh = function () {
    $.ajax({
        url: '/Home/Get',
        type: 'GET',
        success: function (citations) {
            Push(citations);
        }
    });
}

//функция для изменения списка цитат
function Push(citations) {
    viewModel.citations.removeAll();
    var i = 0;
    $.each(citations, function (index, citation) {
        if (citation['category'] == null) viewModel.citations.push(new Citation(
            citation['id'],
            citation['text'],
            citation['author'],
            citation['date'])
        );
        else viewModel.citations.push(new Citation(
            citation['id'],
            citation['text'],
            citation['author'],
            citation['date'],
            citation['category']['name'])
        );
        $('#actions0').attr('id', 'actions' + (i + 1));
        i++;
        $('<a href="/Home/Update/' + citation['id'] + '" class="btn btn-primary">Изменить</a>').appendTo('#actions' + i);

    });
}

//событие на нажатие кнопки Удалить
viewModel.delete = function () {
    viewModel.deleteCitation(this.id);
}

//запрос на удаление записи из БД
viewModel.deleteCitation = function (id) {
    $.ajax({
        url: "Home/Delete/" + id,
        method: "DELETE",
        success: function () {
            viewModel.refresh();
        }
    })
}

//привязка модели
ko.applyBindings(viewModel);

viewModel.refresh();

//поиск цитат, срабатывает при нажатии на клавишу
$('#text').keyup(function () {
    $.ajax({
        url: '/Home/Search',
        type: 'Post',
        data: ({
            text: this.value,
            author: $('#author').val()
        }),
        success: function (citations) {
            Push(citations);
        }
    });
})

$('#author').keyup(function () {
    $.ajax({
        url: '/Home/Search',
        type: 'Post',
        data: ({
            text: $('#text').val(),
            author: this.value
        }),
        success: function (citations) {
            Push(citations);
        }
    });
})

//валидация введенных данных

//при создании цитаты
$('#create-text').keyup(function () {
    if (this.value.length == 0) {
        $('#create-text-required').show();
        $('#create-text-more').hide();
        $('#btn-create').attr('disabled', true);
        $('#create-text').css('border', '2px solid red');
    }
    else if (this.value.length >= 255) {
        $('#create-text-required').hide();
        $('#create-text-more').show();
        $('#btn-create').attr('disabled', true);
        $('#create-text').css('border', '2px solid red');
    }
    else {
        $('#create-text-required').hide();
        $('#create-text-more').hide();
        $('#create-text').css('border', '2px solid green');
        if ($('#create-author').val().length > 0 && $('#create-author').val().length < 100) 
            $('#btn-create').attr('disabled', false);
        else 
            $('#btn-create').attr('disabled', true);
    }
})

$('#create-author').keyup(function () {
    if (this.value.length == 0) {
        $('#create-author-required').show();
        $('#create-author-more').hide();
        $('#btn-create').attr('disabled', true);
        $('#create-author').css('border', '2px solid red');
    }
    else if (this.value.length >= 100) {
        $('#create-author-required').hide();
        $('#create-author-more').show();
        $('#btn-create').attr('disabled', true);
        $('#create-author').css('border', '2px solid red');
    }
    else {
        $('#create-author-required').hide();
        $('#create-author-more').hide();
        $('#create-author').css('border', '2px solid green');
        if ($('#create-text').val().length > 0 && $('#create-text').val().length < 255) 
            $('#btn-create').attr('disabled', false);
        else 
            $('#btn-create').attr('disabled', true);
    }
})

//при изменении цитаты
$('#update-text').keyup(function () {
    if (this.value.length == 0) {
        $('#update-text-required').show();
        $('#update-text-more').hide();
        $('#btn-update').attr('disabled', true);
        $('#update-text').css('border', '2px solid red');
    }
    else if (this.value.length >= 255) {
        $('#update-text-required').hide();
        $('#update-text-more').show();
        $('#btn-update').attr('disabled', true);
        $('#update-text').css('border', '2px solid red');
    }
    else {
        $('#update-text-required').hide();
        $('#update-text-more').hide();
        $('#update-text').css('border', '2px solid green');
        if ($('#update-author').val().length > 0 && $('#update-author').val().length < 100)
            $('#btn-update').attr('disabled', false);
        else
            $('#btn-update').attr('disabled', true);
    }
})

$('#update-author').keyup(function () {
    if (this.value.length == 0) {
        $('#update-author-required').show();
        $('#update-author-more').hide();
        $('#btn-update').attr('disabled', true);
        $('#update-author').css('border', '2px solid red');
    }
    else if (this.value.length >= 100) {
        $('#update-author-required').hide();
        $('#update-author-more').show();
        $('#btn-update').attr('disabled', true);
        $('#update-author').css('border', '2px solid red');
    }
    else {
        $('#update-author-required').hide();
        $('#update-author-more').hide();
        $('#update-author').css('border', '2px solid green');
        if ($('#update-text').val().length > 0 && $('#update-text').val().length < 255)
            $('#btn-update').attr('disabled', false);
        else
            $('#btn-update').attr('disabled', true);
    }
})