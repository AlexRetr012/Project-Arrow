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
	//��������� ��� �������� ����������
	
	/*
	dataGridView1->Rows->Add();//���������� ������
	dataGridView1->Rows[0]->Cells[1]->Value = "D";
	*/
	label4->Hide();

	//
	// ��������� ���������� ������� ������ �� �������� ���� ��� 1 � 2 ����
	//
	//1 ����
	//
	comboBox1->SelectedItem = "C"; // ��������� �� ������� ��������� ���� ������

	return System::Void();
}

void DisksLabels() {

}

System::Void ArrowProject::MyForm::button6_Click(System::Object^ sender, System::EventArgs^ e)
{
	//chart1->Series["Space"]->Points->Add(89); //���������� � ���-���� 
	return System::Void();
}

System::Void ArrowProject::MyForm::button1_Click(System::Object^ sender, System::EventArgs^ e)
{
	//������ ������� �������
	//���-���� ���� �����
	panel12->Hide();
	panel11->Hide();
	panel10->Hide();
	panel9->Show();
	panel7->Hide();
	label4->Show();
	//���-���� ���� ���
	
	label3->Text="������� �������";
	chart1->Series["Series1"]->Points->AddXY("�", 89);
	chart1->Series["Series1"]->Points->AddXY("C", 11);
	chart1->Controls->Clear();
	return System::Void();
}

System::Void ArrowProject::MyForm::button2_Click(System::Object^ sender, System::EventArgs^ e)
{
	//������ ������ ������
	//���-���� ���� �����
	panel12->Hide();
	panel11->Hide();
	panel10->Hide();
	panel9->Hide();
	panel7->Show();
	label4->Show();
	//���-���� ���� ���
	label3->Text = "������ ������";
	
	return System::Void();
}

System::Void ArrowProject::MyForm::button3_Click(System::Object^ sender, System::EventArgs^ e)
{
	//������ ����������
	//���-���� ���� �����
	panel12->Hide();
	panel11->Hide();
	panel10->Show();
	panel7->Hide();
	panel9->Hide();
	label4->Hide();
	//���-���� ���� ���
	label3->Text = "������ ����������";
	return System::Void();
}

System::Void ArrowProject::MyForm::button4_Click(System::Object^ sender, System::EventArgs^ e)
{
	//������ �����������������
	//���-���� ���� �����
	panel12->Hide();
	panel11->Show();
	panel10->Hide();
	panel7->Hide();
	panel9->Hide();
	label4->Hide();
	//���-���� ���� ���
	
	label3->Text = "�����������������";
	return System::Void();
}

System::Void ArrowProject::MyForm::button5_Click(System::Object^ sender, System::EventArgs^ e)
{
	//������ � ���������
	//���-���� ���� �����
	panel12->Show();
	panel11->Hide();
	panel10->Hide();
	panel7->Hide();
	panel9->Hide();
	label4->Hide();
	//���-���� ���� ���
	label3->Text = "����������";
	return System::Void();
}
