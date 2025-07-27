import { Component } from '@angular/core';
import { ChartConfiguration, ChartType } from 'chart.js';

@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {
   user = {
    fullName: 'Dr. Het Shah',
    email: 'dr.hetshah@medguardian.com',
    profileImage: 'assets/Images/userimage.jpeg' // Replace with base64 or image path if available
  };

   // Static medical data for chart
  barChartOptions: ChartConfiguration<'bar'>['options'] = {
    responsive: true,
    plugins: {
      legend: { display: false }
    }
  };

  barChartLabels: string[] = ['Diabetes', 'Hypertension', 'Asthma', 'Heart Disease'];
  barChartData: ChartConfiguration<'bar'>['data'] = {
    labels: this.barChartLabels,
    datasets: [
      {
        data: [3, 2, 1, 1], // Static counts
        label: 'Medical Cases',
        backgroundColor: ['#5DADE2', '#F1948A', '#58D68D', '#F7DC6F']
      }
    ]
  };

   defaultImg = 'https://via.placeholder.com/60x60.png?text=User';

   quickStats = [
    { title: 'My Appointments', desc: 'View and manage appointments', icon: 'bi-calendar' },
    { title: 'My Reports', desc: 'Access health records', icon: 'bi-file-earmark-text' },
    { title: 'Profile Settings', desc: 'Update your information', icon: 'bi-gear' }
  ];

  constructor() {}

  ngOnInit(): void {}
}
