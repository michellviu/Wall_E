using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Walle;
using Point = Walle.Point;

namespace Wall_E
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Parser.Reset();
            // Obtiene la entrada del usuario
            string entrada = TextBox.Text;
            // Limpia el lienzo para dibujar una nueva forma
            lienzo.Children.Clear();

            Interpretar(entrada);

            void Interpretar(string code)
            {
                LexicalAnalyzer lex = Lexer.LexicalAnalyzer;

                List<Token> tokens = lex.GetTokens(code, new List<CompilingError>());
                try
                {
                    foreach (var line in Parser.SeparaPorLinea(tokens))
                    {
                        if (line.Count == 1 && line[0].Value == "restore")
                        {
                            if (Parser.color.Count > 0)
                                Parser.color.Pop();
                            continue;
                        }

                        IExpressionType expresion = Parser.Parse(line);

                        switch (expresion.expressiontype)
                        {
                            //Declaración de una función
                            case ExpressionType.StatementFunction:
                                {
                                    _ = expresion.Evaluate();
                                    StatementFunction funcion = (StatementFunction)expresion;
                                    Parser.funciones.Add(funcion.identificador, funcion);
                                }
                                break;
                            //Declaración de una figura
                            case ExpressionType.StatementShapes:
                                {
                                    IFigure variable = expresion.Evaluate();

                                    if (Parser.variables.ContainsKey(variable.identificador) || Parser.constantes.ContainsKey(variable.identificador))
                                        throw new Exception("! SEMANTIC ERROR: \n Una variable o función llamada '" + variable.identificador + "' ya fue definida.");

                                    Parser.variables.Add(variable.identificador, variable);
                                }
                                break;
                            //Asignaciones
                            case ExpressionType.StatementConst:
                                {
                                    List<IType> consts = expresion.Evaluate();

                                    foreach (IType variable in consts)
                                    {
                                        if (Parser.constantes.ContainsKey(variable.identificador))
                                            Parser.constantes[variable.identificador] = (IType)variable;
                                        else if (Parser.variables.ContainsKey(variable.identificador))
                                        {
                                            if (Parser.variables[variable.identificador].GetType() != variable.GetType())
                                                throw new Exception("! SEMANTIC ERROR: \n No se puede convertir un tipo '" + Parser.variables[variable.identificador].GetType() + "' en '" + variable.GetType() + "'. \n Línea: " + line[0].Location.Line);

                                            Parser.variables[variable.identificador] = (IFigure)variable;
                                        }
                                        else
                                            Parser.constantes.Add(variable.identificador, variable);
                                    }
                                }
                                break;
                            //Instrucción Draw
                            case ExpressionType.Draw:
                                {
                                    Draw draw = (Draw)expresion;
                                    IFigure type = draw.Evaluate();

                                    if (draw.etiqueta != "")
                                        type.etiqueta = draw.etiqueta;

                                    type.color = draw.color;
                                    Parser.draws.Add(type);
                                }
                                break;
                            //Instrucción Import
                            case ExpressionType.Import:
                                {
                                    Interpretar((string)expresion.Evaluate());
                                }
                                break;
                            //Declaración de un color
                            case ExpressionType.Color:
                                {
                                    Parser.color.Push(expresion.Evaluate());
                                }
                                break;
                            //Instrucción Let_in
                            case ExpressionType.LetIn:
                                {
                                    LetIn letIn = (LetIn)expresion;
                                    IType type = letIn.Evaluate();


                                    if (type is Number)
                                    {
                                        Number number = (Number)type;
                                        Parser.prints.Add(number.value.ToString());
                                    }
                                    else if (type is Booleano)
                                    {
                                        Number number = (Number)type;
                                        Parser.prints.Add(number.value.ToString());
                                    }
                                    else
                                        Parser.prints.Add(type.ToString()!);

                                }
                                break;
                            //Instrucción If_Then_Else
                            case ExpressionType.Conditional:
                                {
                                    If_else ifElse = (If_else)expresion;
                                    IType type = ifElse.Evaluate();

                                    if (type is Number)
                                    {
                                        Number number = (Number)type;
                                        Parser.prints.Add(number.value.ToString());
                                    }
                                    else if (type is Booleano)
                                    {
                                        Number number = (Number)type;
                                        Parser.prints.Add(number.value.ToString());
                                    }
                                    else
                                        Parser.prints.Add(type.ToString()!);

                                }
                                break;
                            //Instrucción Print
                            case ExpressionType.Print:
                                {
                                    Print print = (Print)expresion;
                                    IType type = print.Evaluate();


                                    if (type is Number)
                                    {
                                        Number number = (Number)type;
                                        Parser.prints.Add(number.value.ToString());
                                    }
                                    else if (type is Booleano)
                                    {
                                        Number number = (Number)type;
                                        Parser.prints.Add(number.value.ToString());
                                    }
                                    else
                                        Parser.prints.Add(type.ToString()!);

                                }
                                break;
                        }
                    }

                    foreach (var figure in Parser.draws)
                    {
                        figure.Dibuja(lienzo);
                    }
                    foreach (var print in Parser.prints)
                    {
                        Output.Text = Output.Text + " " + print;
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }


            }





        }


        private void Click_Guardar(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                string nombreArchivo = saveFileDialog.FileName;
                string contenido = TextBox.Text;

                try
                {
                    File.WriteAllText(nombreArchivo, contenido);
                    MessageBox.Show("Contenido guardado con éxito en " + nombreArchivo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar el archivo: " + ex.Message);
                }
            }

        }
        private void Click_Abrir(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string contenido = File.ReadAllText(openFileDialog.FileName);
                TextBox.Text = contenido;
            }
        }

        private void Click_Limpiar(object sender, RoutedEventArgs e)
        {
            TextBox.Clear();
        }



    }
}

