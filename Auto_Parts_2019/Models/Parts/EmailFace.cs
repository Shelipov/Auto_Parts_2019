using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auto_Parts_2019.Models.Parts
{
    public static class EmailFace
    {
        public const string Up = @"<html>
<head>
    <meta charset='utf-8' />
    <environment exclude='Development'>
        <link rel='stylesheet' href='https://ajax.aspnetcdn.com/ajax/bootstrap/3.3.7/css/bootstrap.min.css'
    </environment>
</head>
<body>
    <nav class='navbar navbar-inverse navbar-fixed-top'>
        <div class='container'>
            <div class='navbar-header'>
                <h3><a class='navbar-brand'>Компания ООО 'Тех-Трейд' официальный представитель завода Otto-Zimmermann в Украине.</a></h3>
				<h3><a class='navbar-brand'>т. 050-725-93-53,068-230-56-53</a></h3>
            </div>
           
        </div>
    </nav>
<body>";
        public const string Down = @"<div class='container body-content'>
        <hr />
		<br><br>
		<strong>Адрес:</strong><br />
    г. Киев ул. Дегтяревская 21<br />
    <strong>Отдел продаж:</strong><br />
    <abbr title='Phone'><strong>+38-050-725-93-53</strong></abbr><br />
    <abbr title='Phone'><strong>+38-068-230-56-53(Viber)</strong></abbr><br />
    <strong>ICQ:</strong> <a href='mailto:https://icq.com/windows/ru'>Тут будет ICQ</a><br />
    <strong>Skype:</strong> <a href='mailto:https://www.skype.com/ru/get-skype/'>Тут будет Skype</a><br />
    <strong>Sales:</strong> <a href='mailto:v.koguchniy@ttua.com.ua'>v.koguchniy@ttua.com.ua</a><br />
    <strong>Sales:</strong> <a href='mailto:o.lugovoy@ttua.com.ua'>o.lugovoy@ttua.com.ua</a><br />
    <strong>Бухгалтерия:</strong> <a href='mailto:s.lapchenko@ttua.com.ua'>s.lapchenko@ttua.com.ua</a><br />
    <strong>Склад:</strong> <a href='mailto:sklad@ttua.com.ua'>sklad@ttua.com.ua</a><br />
    <strong>Прайс листы:</strong> <a href='mailto:price@ttua.com.ua'>price@ttua.com.ua</a><br />
	<br><br>
        <footer>
            <p>&copy; Разработчик <a href='mailto:sergeshelipov@gmail.com'>sergeshelipov@gmail.com</a></p>
        </footer>
    </div>";
    }
}
