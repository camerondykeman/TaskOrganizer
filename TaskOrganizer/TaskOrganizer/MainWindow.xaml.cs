/*
 * Cameron Dykeman
 * 01/06/2013
 * Project #2 for GUI Programming with .NET
 * 
 * A simple Task Organizer with AutoSave feature.
 * Allows for creation, tracking, editing, and deletion of tasks.
 * All activity is automatically saved to file without any input needed by the user.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;

namespace TaskOrganizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private List<Task> tasks = new List<Task>();
        private Task currentTask;

        public MainWindow()
        {
            InitializeComponent();
        }

        //operations to perform on window first loading
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //populate comboBox
            comboBox1.Items.Add("Low");
            comboBox1.Items.Add("Medium");
            comboBox1.Items.Add("High");
            //load from file
            load();
            //set selection
            currentTask = tasks[0];
        }

        //change of selection in treeview
        private void treeViewTasks_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                //save data
                save();
                //make sure controls are visible
                gridContent.Visibility = System.Windows.Visibility.Visible;
                //close expanders
                expanderDesc.IsExpanded = false;
                expanderDetails.IsExpanded = false;
                expanderPriority.IsExpanded = false;
                expanderDates.IsExpanded = false;
                expanderStatus.IsExpanded = false;

                //get treeview item's header
                TreeViewItem selectedItem = (TreeViewItem)treeViewTasks.SelectedItem;
                String selectedName = selectedItem.Header.ToString();

                //loop through tasks to find given task
                foreach (Task task in tasks)
                {
                    if (task.name.CompareTo(selectedName) == 0)
                    {
                        currentTask = task;
                        displayTask(task);
                    }
                }
            }
            catch (Exception ex)
            {
                //No more tasks in list, hide controls
                gridContent.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        //Details expander operations
        private void expanderDetails_Expanded(object sender, RoutedEventArgs e)
        {
            textBoxDetails.Text = textBlockDetails.Text;
            textBlockDetails.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void expanderDetails_Collapsed(object sender, RoutedEventArgs e)
        {
            textBlockDetails.Visibility = System.Windows.Visibility.Visible;
        }

        //Description expander operations
        private void expanderDesc_Expanded(object sender, RoutedEventArgs e)
        {
            textBoxDesc.Text = textBlockDesc.Text;
            textBlockDesc.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void expanderDesc_Collapsed(object sender, RoutedEventArgs e)
        {
            textBlockDesc.Visibility = System.Windows.Visibility.Visible;
        }

        //Displays content from the given task
        private void displayTask(Task task)
        {
            //set pannel headers
            //expanderDesc.Header = task.name;
            textBlockDesc.Text = task.description;
            textBlockDetails.Text = task.details;
            expanderPriority.Header = task.priority + " Priority";
            expanderDates.Header = "Due " + task.dateDue;
            expanderStatus.Header = task.status;

            //set Priority pannel
            foreach (String item in comboBox1.Items)
            {
                String asdf = item;
                if (item.CompareTo(task.priority) == 0)
                {
                    comboBox1.SelectedItem = asdf;
                }
            }

            //set Status pannel
            if (task.status.CompareTo("Planning")==0)
            {
                radioButtonPlanning.IsChecked = true;
            }
            else if (task.status.CompareTo("In Progress") == 0)
            {
                radioButtonInProgress.IsChecked = true;
            }
            else if (task.status.CompareTo("Finished") == 0)
            {
                radioButtonFinished.IsChecked = true;
            }
            else if (task.status.CompareTo("Blocked") == 0)
            {
                radioButtonBlocked.IsChecked = true;
            }
            
            //set Dates pannel
            try
            {
                datePickerStarted.SelectedDate = DateTime.Parse(task.dateStarted);
                datePickerDue.SelectedDate = DateTime.Parse(task.dateDue);
            }
            catch (Exception exep)
            {
                datePickerStarted.SelectedDate = DateTime.Now;
                datePickerDue.SelectedDate = DateTime.Now;
            }

        }
        private void newTaskLayout()
        {
            //layout
            expanderDesc.Header = "Description";
            expanderDesc.IsExpanded = true;
            expanderDetails.Header = "Details";
            expanderDetails.IsExpanded = true;
            expanderPriority.Header = "Set a Priority";
            expanderPriority.IsExpanded = true;
            expanderDates.Header = "Map a Timeline";
            expanderDates.IsExpanded = true;
            expanderStatus.Header = "Status";
            expanderStatus.IsExpanded = true;
            radioButtonPlanning.IsChecked = true;

            //clear old values
            textBlockDesc.Text = "Add a Descriptions";
            textBlockDetails.Text = "Give some Details";
            textBoxDesc.Text = "Add a Descriptions";
            textBoxDetails.Text = "Give some Details";
            comboBox1.SelectedItem = -1;
            datePickerDue.SelectedDate = DateTime.Now;
            datePickerStarted.SelectedDate = DateTime.Now;
        }

        //Radio buttons
        private void radioButtonPlanning_Checked(object sender, RoutedEventArgs e)
        {
            currentTask.status = "Planning";
        }
        private void radioButtonInProgress_Checked(object sender, RoutedEventArgs e)
        {
            currentTask.status = "In Progress";
        }
        private void radioButtonBlocked_Checked(object sender, RoutedEventArgs e)
        {
            currentTask.status = "Blocked";
        }
        private void radioButtonFinished_Checked(object sender, RoutedEventArgs e)
        {
            currentTask.status = "Finished";
        }

        //Delete Task button
        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Delete this task?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                
                tasks.Remove(currentTask);
                save();
                treeViewTasks.Items.Remove(treeViewTasks.SelectedItem);
            }
        }

        //New Task button
        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            newTaskPrompt();
        }

        //prompts the user for new task name
        private void newTaskPrompt()
        {
            NewTaskPrompt dialog = new NewTaskPrompt();

            if (dialog.ShowDialog() == true)
            {
                newTask(dialog.ResponseText);
            }
        }

        //creates a new task with the given name
        private void newTask(String name){
            //save current task
            save();
            //create a new task object
            Task newTask = new Task("newTask", "Add a Description", DateTime.Now.ToString(), DateTime.Now.ToString(), "Planning", "Low", "Give some Details");
            //give it the name
            newTask.name = name;
            //place new task into list of tasks
            tasks.Add(newTask);
            //place new task into treeview
            TreeViewItem newTreeItem = new TreeViewItem();
            newTreeItem.Header = newTask.name;
            treeViewTasks.Items.Add(newTreeItem);
            //make new task current selection
            foreach (TreeViewItem item in treeViewTasks.Items)
            {
                String header = item.Header.ToString();
                String taskname = newTask.name;
                if (item.Header.ToString().CompareTo(newTask.name) == 0)
                {
                    item.IsSelected = true;
                }
            }
            //new task layout
            newTaskLayout();
            //make new task the current task
            currentTask = newTask;
        }

        //replace old description with new
        private void textBoxDesc_TextChanged(object sender, TextChangedEventArgs e)
        {
            currentTask.description = textBoxDesc.Text;
        }
        //replace old details with new
        private void textBoxDetails_TextChanged(object sender, TextChangedEventArgs e)
        {
            currentTask.details = textBoxDetails.Text;
        }
    
        //save to file
        private void save()
        {
            //delete old file
            //System.IO.File.Delete("C://Tasks.txt");
            //write new file
            System.IO.StreamWriter sr = new System.IO.StreamWriter("C://Tasks.txt");
            foreach (Task task in tasks)
            {
                sr.WriteLine("Name[" + task.name + "]");
                sr.WriteLine("Description[" + task.description + "]");
                sr.WriteLine("DateStarted[" + task.dateStarted + "]");
                sr.WriteLine("DateDue[" + task.dateDue + "]");
                sr.WriteLine("Status[" + task.status + "]");
                sr.WriteLine("Priority[" + task.priority + "]");
                sr.WriteLine("Details[" + task.details + "]");
            }
            sr.Close();
        }
        //load from file
        private void load()
        {
            //ADD ITEMS TO TREEVIEW
            //read in list from file
            try
            {
                //treeViewTasks.Items.SortDescriptions.Add(new SortDescription("Header", ListSortDirection.Ascending));
                System.IO.StreamReader sr = new System.IO.StreamReader("C://Tasks.txt");
                String line = "";
                //String taskName = "";
                //Task currentTask = new Task();
                //move line by line
                while ((line = sr.ReadLine()) != null)
                {
                    //parse line into key and value
                    string[] wordArray = line.Split('[', ']');
                    String key = wordArray[0];
                    String val = wordArray[1];

                    //if its the name of a task, add the old task, then start a new task
                    if (key.CompareTo("Name") == 0)
                    {
                        currentTask = new Task();
                        currentTask.name = val;
                    }
                    //else add the value to the task
                    else
                    {
                        switch (key)
                        {
                            case "Description":
                                currentTask.description = val;
                                break;
                            case "DateStarted":
                                currentTask.dateStarted = val;
                                break;
                            case "DateDue":
                                currentTask.dateDue = val;
                                break;
                            case "Status":
                                currentTask.status = val;
                                break;
                            case "Priority":
                                currentTask.priority = val;
                                break;
                            case "Details":
                                currentTask.details = val;
                                //Last value in task, add task to list
                                tasks.Add(currentTask);
                                break;
                        }
                    }
                }
                //add tasks to tree view
                foreach (Task task in tasks)
                {
                    TreeViewItem name = new TreeViewItem() { Header = task.name };
                    treeViewTasks.Items.Add(name);
                }
                //set up treeview
                treeViewTasks.Items.SortDescriptions.Add(new SortDescription("Header", ListSortDirection.Ascending));
                treeViewTasks.Items.Refresh();
                sr.Close();
            }
            catch (Exception ex)
            {
                //Failed to load file
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK);
            }
            
        }

        //window exit operations
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            save();
        }
    }
}
