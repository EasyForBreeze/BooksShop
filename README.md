1) get - возвращает полный список товаров, по умолчанию отсортированный по Id
			Флаги для реализации:
				--title=%% учитывать товар в выдаче, только если указанная подстрока встречается в названии, если таких товаров нет, написать сообщение
				--author=%% учитывать товар в выдаче, только если указанная подстрока встречается в поле автор, если таких товаров нет, написать сообщение
				--date=%% аналогично, но для даты, дата в формате yyyy-MM-dd, если дата указана неправильно, вывести текст ошибки
				--order-by=[title|author|date|count] отсортировать список товаров по указанному полю, если указано неправильное поле, то вывести текст ошибки
			Флаги могут использоваться одновременно, например: `get --title "для чайников" --order-by="count"` - на выходе должен быть список товаров,
				в названии которых значатся слова "для чайников", отсортированных по их количеству в базе.
				
		2) buy - уменьшает количество указанных товаров на 1
			Флаги для реализации:
				--id=%% Id товара, который нужно купить
				
		3) restock - увеличивает случайное количество случайных товаров на случайное положительное число. Если флагом указаны ID или количество, то пополнить в соответствии с заданными флагами.
			Флаги для реализации:
				--id=%% Id товара, количество которого нужно пополнить
				--count=%% Число, на которое нужно увеличить количество товара
   4) adddata - добавления книг в БД
