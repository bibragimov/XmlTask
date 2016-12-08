using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using XmlTask.Dto;
using XmlTask.Services;
using XmlTask.Utils;

namespace XmlTask.ViewModels
{
    public class MainViewModel : Notifier
    {
        private readonly SqlDataService _sqlDataService;
        private string _filePath;
        private bool _isEnableSaveInBd;
        private string _messageErorr;

        public MainViewModel()
        {
            _sqlDataService = new SqlDataService();
            SelectFileCommand = new RelayCommand(SelectFileMethod);
            SaveFileCommand = new RelayCommand(SaveFileMethod);
            SaveInBdCommand = new RelayCommand(SaveInBdMethod);
            IsEnableSaveInBd = false;
            Files = new ObservableCollection<FileModel>();
            ShowDataCommand = new RelayCommand(ShowDataMethod);
            EditedRowCommand = new RelayCommand<DataGrid>(UpdateFileInfoMethod);
        }

        /// <summary>
        ///     Команда выбора файла
        /// </summary>
        public ICommand SelectFileCommand { get; set; }

        /// <summary>
        ///     Команда сохранения данных из Бд в файл
        /// </summary>
        public ICommand SaveFileCommand { get; set; }

        /// <summary>
        ///     Команда загрузка данных из файла в БД
        /// </summary>
        public ICommand SaveInBdCommand { get; set; }

        /// <summary>
        ///     Отображение данных из БД
        /// </summary>
        public ICommand ShowDataCommand { get; set; }


        public ICommand EditedRowCommand { get; set; }

        /// <summary>
        ///     Колекция записей ин-ции о файлах
        /// </summary>
        public ObservableCollection<FileModel> Files { get; set; }

        /// <summary>
        ///     Путь до файла
        /// </summary>
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Сообщения с ошибками
        /// </summary>
        public string MessageErorr
        {
            get { return _messageErorr; }
            set
            {
                _messageErorr = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Свойство доступности кнопки для загрузки данных в БД
        /// </summary>
        public bool IsEnableSaveInBd
        {
            get { return _isEnableSaveInBd; }
            set
            {
                _isEnableSaveInBd = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Метод выбора файла
        /// </summary>
        private void SelectFileMethod()
        {
            var myDialog = new OpenFileDialog
            {
                Filter = "Xml Files|*.xml",
                CheckFileExists = true
            };

            if (myDialog.ShowDialog() == true)
            {
                MessageErorr = string.Empty;

                FilePath = myDialog.FileName;

                Logger.Log.Info("Получен файл: " + FilePath);
                Logger.Log.Info("Проверка имени файла на формат:");

                var re = new Regex(RegexExtension.FileNameRegex);
                var mc = re.Match(_filePath);

                if (!mc.Success)
                {
                    Logger.Log.Info("Файл не соответствует формату");
                    MessageErorr = "Файл не соответствует формату. Выберите новый файл";
                    IsEnableSaveInBd = false;
                }
                else
                {
                    IsEnableSaveInBd = true;
                }
            }
        }

        /// <summary>
        ///     Метод сохранение в файл из БД;
        /// </summary>
        private void SaveFileMethod()
        {
            Logger.Log.Info("Сохранение файла");

            var dto = _sqlDataService.GetFiles();

            if (dto.Files.Count == 0)
            {
                Logger.Log.Info("В Бд нет записей");
                MessageBox.Show("В Бд нет записей");
                return;
            }

            var xmlFile = XmlParser.SerializeObject(dto);

            var saveFileDialog = new SaveFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                DefaultExt = ".xml",
                Filter = "Xml Files|*.xml"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, xmlFile);
                Logger.Log.Info(string.Format("Сохранено в файл {0}", saveFileDialog.FileName));
            }
        }

        /// <summary>
        ///     Метод загрузки данных из файла в БД
        /// </summary>
        private void SaveInBdMethod()
        {
            Logger.Log.Info("Загрузка данных из файла в Бд");

            var list = XmlParser.ParseXml<FilesDto>(FilePath, "Files");

            Logger.Log.Info(string.Format("Количество записей в файле: {0}", list.Files.Count));

            if (list.Files.Count > 0)
                _sqlDataService.InsertEntities(list);
        }

        /// <summary>
        ///     Метод отображения данных из Бд
        /// </summary>
        private void ShowDataMethod()
        {
            Logger.Log.Info("Отображение данных из Бд");

            var models = _sqlDataService.GetFileModels();

            models.ForEach(x => Files.Add(x));
        }

        /// <summary>
        ///     Метод обновления инф-ции о файле
        /// </summary>
        /// <param name="grid"></param>
        private void UpdateFileInfoMethod(DataGrid grid)
        {
            var model = grid.SelectedItem as FileModel;

            Logger.Log.Info("Обновление файла с Id = " + model.Id);

            _sqlDataService.UpdateFileInfo(model);
        }
    }
}