# Adventure PRO

**Adventure PRO** - это программа, которая позволяет пользователю составить туристический тур, в который включены развленчения и проживание для выбранного направления, а также авиабилеты.

>Исходный код программы расположен по адресу https://github.com/SharonDaimon/AdventurePRO

## Описание пользовательского интерфейса

При запуске программы отобразится такое окно:
![alt tag](http://upload.akusherstvo.ru/image971872.png)

Выберите дату начала и окончания поездки, вашу страну, ваш город, страну, куда вы едете и туристическое направление, куда вы едете. Когда вы выбираете страну, список доступных направлений в этой стране загружается автоматически.

![alt tag](http://upload.akusherstvo.ru/image971882.png)

Затем добавте участников путешествия. Чтобы добавить участника, нажмите кнопку "+"

![alt tag](http://upload.akusherstvo.ru/image971884.png)

Когда вы выберете место, куда вы едете, загрузится список событий в этом месте. Выберите интересные вам.

![alt tag](http://upload.akusherstvo.ru/image971888.png)

Теперь нажмите кнопку "search"

![alt tag](http://upload.akusherstvo.ru/image971889.png)

Внизу отобразится составленное путешествие

![alt tag](http://upload.akusherstvo.ru/image971890.png)

Имеется виджет для просмотра прогноза погоды. Перемещая ползунок, вы меняете день, для которого отображается прогноз.

![alt tag](http://upload.akusherstvo.ru/image971893.png)

## Роли участников команды разработки

**[Настя]** - занимается разработкой модели данных, логики приложения и тестов

**[Кристина]** - работает с API и базами данных

**[Катя]** - делает пользовательский интерфейс

## Описание классов программы

Диаграмма классов программы расположена [здесь]

Следующие классы представляют модель данных:
*    **DbItem** - описывает сущность, которая может храниться в базе данных, т.е. по существу, любую сущность можели данных
*   **Acquirable** - описывает нечто, что можно приобрести (билет на самолет, номер в гостинице)
*   **Adventure** - описывает путешествие, которое было составлено программой
*   **Attraction** - представляет туристическую достопримечательность или мероприятие
*   **Comment** - написанный в Интернете комментарий
*   **Country** - описывает некоторое государство
*   **StaticCurrencyConverter** - статический класс, который испольщуется для конвертации валют
*   **ICurrencyConverter** - представляет конвертер валют
*   **Destination** - туристическое направление ( как правило, город)
*   **Hotel** - описывает некоторую гостиницу
*   **Location** - структура, содержащая географические координаты некоторого объекта - широту и долготу
*   **Nameable** - что-то, у чего есть имя (например, человек)
*   **Occupancy** - размещение в гостиничном номере группы людей
*   **OnlineDescribed** - представляет сущность, которая содержит описание в Интернете
*   **Person** - описывает человека - члена поездки
*   **Taxi** - описывает поездку на такси
*   **Ticket** - описывает билет на самолет (может описывать и билет на другой вид транспорта)
*   **Weather** - описывает погоду в определенное время в определённом месте

Также имеется класс **AdventureDbContext**, который служит для сохранения составленных путешествий в базу данных

Логику приложения описывают следующие классы:
*   **AdventureOptions** - класс, который содержит параметры путешествия и формирует набор некоторых параметров для выбора (например, список городов во введенной стране, в которые можно поехать) на основе уже введенных
*   **AdventureApiContext.cs** - содержит логику составления путешествия на основе введенных параметров

Следующие классы отвечают за работу с API одноименных с ними сервисов: **Fixer**, **Hotelbeds**, **Openweathermap**, **QPX**, **Seatwave**.
Класс **HttpManager** отвечает за отправку GET и POST запросов на указанный адрес с указанными параметрами и заголовками запроса и, в случае Http POST запроса - телом запроса.

Класс **Accomodation** (папка APIs/Options проекта AdventurePRO.Model) содержит информацию о том, как размещать людей в номере гостиницы.

Следующие классы содержатся в папке APIs/Results проекта AdventurePRO.Model и описывают результаты вызовов методотов доступа к API:
*   **HotelRoom** - представляет некторое размещение гостей в гостиничном номере
*   **QPXTrip** - содредит билеты "туда" и обратно
*   **SeatwaveEvent** - описывает некоторое событие в городе, в который пользователь собирается поехать
*   **SeatwaveVenue** - место, где проводится событие, описанное выше

Проект **AdventurePRO.Tests** полностью состоит из классов модульных тестов. Проект **AdventurePRO** полностью состоит из представлений и конвертеров значений для WPF привязки данных

   [repository]: <https://github.com/SharonDaimon/AdventurePRO>
   [Настя]:<https://github.com/SharonDaimon>
   [здесь]:<https://github.com/SharonDaimon/AdventurePRO/blob/master/AdventurePRO.Model/ClassDiagram1.cd>
   [Кристина]:<https://github.com/Yenikeeva>
   [Катя]:<https://github.com/kekskaty>
