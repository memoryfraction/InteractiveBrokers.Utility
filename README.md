# InteractiveBrokers.Utility
## Abstract

Call IB API to build your own trading program, access to market data and charts, view your own account data. This paper uses C# language to develop the API of IB to obtain the information of the specified account info. 



## Package Usage

### Install Nuget Package

```c#
Install-Package InteractiveBrokers.Utilities -Version 1.0.0
```



### Code

```C#
		//event definition
		private static void IBUtility_AccountSummaryFetched(object sender, EventArgs e)
        {
            AccountSummary summary = (AccountSummary)sender;
            double avgSecConsumed = sw.Elapsed.TotalSeconds / (double)i;
            System.Console.WriteLine("IBBrokers account summary got.index:" + i + "\r\n average second per time:" + avgSecConsumed);
        }

		 // Subscribe the event
         IBUtility.AccountSummaryFetched += IBUtility_AccountSummaryFetched;

 System.Console.WriteLine("index:" + i);
          //begin the request, and the result will be got in IBUtility_AccountSummaryFetched
          IBUtility.GetAccountSummary();             
          while (IBUtility.IsBusy)
              continue;
          IBUtility.Dispose();
```



## IB Introduction

Interactive Brokers Inc. (" Interactive Brokers ") is one of the largest internet-based brokerages in the United States by revenue and trading volume. The company was listed on NASDAQ in May 2007 under the symbol "IBKR" and currently has a combined capital of approximately $5 billion.S&P has a higher rating than Bank of America, Citibank and others.As of the end of November 2016, Interactive Brokers' clients had total assets of approximately $84.6 billion.Interactive Brokers is the first American online securities firm to register with the China Securities Regulatory Commission, accept the supervision of the Chinese mainland and set up a representative office. It is also the only one at present

![img](https://pic2.zhimg.com/80/v2-840ba12d8b33e4f3073d51b666369939_720w.jpg)

Why C#?

![img](https://pic1.zhimg.com/80/v2-bb59e9d8317c7c7d98098d6c4646881c_720w.jpg)



The figure implies that C# is the preferred development language, where the documentation is the most comprehensive.

Ib c # development of the official document is as follows: https://www.interactivebrokers.com/en/index.php?f=11698

Given the millions of trading software available every minute, it's best to choose a well-documented solution.



## 2 Development Steps

### 2.1 Download and install IB API

The API call of IB is essentially a call to the running TWS of IB by the developer, rather than a direct use of IB services.Why is it designed this way?Presumably there may be safety concerns.

So, we first need to download the IB API, address: http://interactivebrokers.github.io/ readers, please according to select the appropriate version in different operating systems.It is highly recommended to choose the stable version.

![img](https://pic1.zhimg.com/80/v2-cd18ff4e0f168320d01b31958aefe3b4_720w.jpg)

After downloading, you can get "TWS API Install 972.18.msi".The source code for the two sample projects is available in the installation directory after installation.One is WinForm and the other is ConsoleApplication.This article uses ConsoleApplication as an example.

![img](https://pic2.zhimg.com/80/v2-e130881e0c16680f58b9f376edda3615_720w.jpg)

### **2.2 Download and Install TWS**

TWS download address：[https://www.interactivebrokers.com/en/index.php?f=16042](https://link.zhihu.com/?target=https%3A//www.interactivebrokers.com/en/index.php%3Ff%3D16042)

input credential to login TWS

![img](https://pic1.zhimg.com/80/v2-9d7095152a56f911740cf6e5f85d4c40_720w.jpg)

Got below printscreen after the login.

![img](https://pic4.zhimg.com/80/v2-02e20a936532828d44c87c83ad2fccc7_720w.jpg)

Note that using the 2.1 consoleApplication call directly at this point will report an error.Error general content: "server initiatively rejects request".Solution: Use IntraX, Tools, and Configuration, check and apply as shown below.

![preview](https://pic4.zhimg.com/v2-dceb23aab0654794f2a43afbe1ca8fc7_r.jpg)

After clicking Apply, you can connect normally using the IB API Console Application.

### 2.3 Understand and use the IB API

When you open the testbed. SLN project, you see the figure below, where it is not recommended to try to modify the contents of cSharpapi [2].

![img](https://pic4.zhimg.com/80/v2-16f36cded7a13cbbf2299df9adacc397_720w.jpg)

Background:

EWrapper: Similar to the SPI class in CTP, it provides a callback function

EclientSocket: Similar to the API class in CTP, it provides active functions

EreaderosSignal: A semaphore that the callback management thread needs to listen for and that is fired when the socket receives data

EReader: When the above semaphore is triggered, the user needs to call the processing function in EReader to trigger the corresponding callback function in EWrapper

Real-time account interface: 9496, simulation account interface: 9497;

In the Testbed project, under program. cs, private static void AccountOperations (eclientSocket Client) has a large number of methods that we need.This article only talks about getting the AccountSummary method, signed as follows:

client.reqAccountSummary(9001, "All", AccountSummaryTags.GetAllTags());

Due to the multithreading of Main, developers are not immune to several problems:

1 How do I know how long it took Testbed to communicate with TWS and get all the AccountSummary information

2 How to communicate across threads to get the final result AccountSummary?

This article presents a possible, but not perfect, solution:

1 Define AccountSummary as shown in the figure below

![img](https://pic3.zhimg.com/80/v2-ecd442641c48d17e29db2860d734e376_720w.jpg)

2 EWRAPPerImpl class. In the AccountSummary method, an AccountSummary is considered to have been assigned when the Account is counted to 21 properties.It works, but it's not perfect.If IB updates the account information, the number of attributes may exceed 21

3 Since the communication between multiple threads is involved, there are three solutions: events, mutex and semaphore.The author uses the event mode to implement.That is, defined in eWrapPerImpl

public event EventHandler AccountSummaryFetched; In the execution of the following method, if it is called more than 21 times, an AccountSummary____ event is triggered.

public virtual void accountSummary(int reqId, string account, string tag, string value, string currency)

4 Subscribe to events in the Main function and get the result of the AccountSummary



## 3 summary

Based on IB API, this paper uses C# to develop an application program to obtain account information successfully.If GitHub reaches 100, it will continue to use its spare time to improve Utilities by making orders, accessing real-time data and so on.



## 4 Reference Materials

[[1\]Interactive Brokers -- Introduction]: https://zhuanlan.zhihu.com/p/26287367
[[2\]A very simple SDK for IB API]: https://zhuanlan.zhihu.com/p/22864483

