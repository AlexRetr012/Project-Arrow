#include "MyForm.h"
#include <Windows.h>
#include <filesystem>

using namespace ArrowProject;

[STAThread] // enables FileDialog

int WINAPI WinMain(HINSTANCE, HINSTANCE, LPSTR, int)
{
	Application::EnableVisualStyles();
	Application::SetCompatibleTextRenderingDefault(false);
	Application::Run(gcnew MyForm);
	return 0;
}


System::Void ArrowProject::MyForm::MyForm_Load(System::Object^ sender, System::EventArgs^ e)
{
	//Поведение при открытии приложения
	
	/*
	dataGridView1->Rows->Add();//добавление строки
	dataGridView1->Rows[0]->Cells[1]->Value = "D";
	*/
	label4->Hide();

	//
	// Прописать вычисление размера дисков до открытия окон для 1 и 2 окна
	//
	//1 окно
	//
	comboBox1->SelectedItem = "C"; // Выставить по дефолту системный диск первым

	return System::Void();
}

void DisksLabels() {

}

System::Void ArrowProject::MyForm::button6_Click(System::Object^ sender, System::EventArgs^ e)
{
	//chart1->Series["Space"]->Points->Add(89); //Добавление в пай-чарт 
	return System::Void();
}

System::Void ArrowProject::MyForm::button1_Click(System::Object^ sender, System::EventArgs^ e)
{
	//Кнопка статуса системы
	//Шоу-хайд лист старт
	panel12->Hide();
	panel11->Hide();
	panel10->Hide();
	panel9->Show();
	panel7->Hide();
	label4->Show();
	//Шоу-хайд лист энд
	
	label3->Text="Статусе системы";
	chart1->Series["Series1"]->Points->AddXY("З", 89);
	chart1->Series["Series1"]->Points->AddXY("C", 11);
	chart1->Controls->Clear();
	return System::Void();
}

System::Void ArrowProject::MyForm::button2_Click(System::Object^ sender, System::EventArgs^ e)
{
	//Кнопка Выбора Режима
	//Шоу-хайд лист старт
	panel12->Hide();
	panel11->Hide();
	panel10->Hide();
	panel9->Hide();
	panel7->Show();
	label4->Show();
	//Шоу-хайд лист энд
	label3->Text = "Выборе режима";
	
	return System::Void();
}

System::Void ArrowProject::MyForm::button3_Click(System::Object^ sender, System::EventArgs^ e)
{
	//Кнопка исключений
	//Шоу-хайд лист старт
	panel12->Hide();
	panel11->Hide();
	panel10->Show();
	panel7->Hide();
	panel9->Hide();
	label4->Hide();
	//Шоу-хайд лист энд
	label3->Text = "Файлах Исключений";
	return System::Void();
}

System::Void ArrowProject::MyForm::button4_Click(System::Object^ sender, System::EventArgs^ e)
{
	//Кнопка администрирования
	//Шоу-хайд лист старт
	panel12->Hide();
	panel11->Show();
	panel10->Hide();
	panel7->Hide();
	panel9->Hide();
	label4->Hide();
	//Шоу-хайд лист энд
	
	label3->Text = "Администрировании";
	return System::Void();
}

System::Void ArrowProject::MyForm::button5_Click(System::Object^ sender, System::EventArgs^ e)
{
	//Кнопка о программе
	//Шоу-хайд лист старт
	panel12->Show();
	panel11->Hide();
	panel10->Hide();
	panel7->Hide();
	panel9->Hide();
	label4->Hide();
	//Шоу-хайд лист энд
	label3->Text = "Информации";
	return System::Void();
}
