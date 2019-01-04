using System;
using System.Linq;
using NBitcoin;
using QBitNinja.Client;
using QBitNinja.Client.Models;

namespace CodeProgrammingBlockchain
{
  class Program
    {
    static void Main(string[] args)
    {
//parse tx ID from transactionResponse and transaction.GetHash
      QBitNinjaClient client = new QBitNinjaClient(Network.TestNet);
      var transactionId = uint256.Parse("8ce2c18154caa2fa4053a7f8776c662a3216beae344891863b1341ead737239f");
      GetTransactionResponse transactionResponse = client.GetTransaction(transactionId).Result;
      NBitcoin.Transaction transaction = transactionResponse.Transaction;
      // Console.WriteLine(transactionResponse.TransactionId);
      // Console.WriteLine(transaction.GetHash());

//show received coin of transaction
      var receivedCoins = transactionResponse.ReceivedCoins;
      foreach (var coin in receivedCoins)
      {
        Money amount = (Money) coin.Amount;

        // Console.WriteLine(amount.ToDecimal(MoneyUnit.BTC));
        var paymentScript = coin.TxOut.ScriptPubKey;
        // Console.WriteLine(paymentScript);
        var address = paymentScript.GetDestinationAddress(Network.TestNet);
        // Console.WriteLine(address);       
      }

//show spent coins of transaction
      var spentCoins = transactionResponse.SpentCoins;
      foreach (var coin in spentCoins)
      {
        Money amount = (Money) coin.Amount;

      //   Console.WriteLine(amount.ToDecimal(MoneyUnit.BTC));
        var paymentScript = coin.TxOut.ScriptPubKey;
      //   Console.WriteLine(paymentScript);
        var signAddress = paymentScript.GetSignerAddress(Network.TestNet);
      //   Console.WriteLine(signAddress);
      }

//Outputs NBitcoin transaction class
      var outputs = transaction.Outputs;
      foreach (TxOut output in outputs)
      {
        Money amount = output.Value;

      //   Console.WriteLine(amount.ToDecimal(MoneyUnit.BTC));
        var paymentScript = output.ScriptPubKey;
      //   Console.WriteLine(paymentScript);
        var address = paymentScript.GetDestinationAddress(Network.Main);
      //   Console.WriteLine(address);
      }

//Inputs NBitcoin transaction class
      var inputs = transaction.Inputs;
      foreach (TxIn input in inputs)
      {
        OutPoint previousOutpoint = input.PrevOut;
      //   Console.WriteLine(previousOutpoint.Hash);
      //   Console.WriteLine(previousOutpoint.N);
      }

//Outpoint = txID + ordering
      OutPoint firstOutPoint = receivedCoins.First().Outpoint;
      // Console.WriteLine(firstOutPoint.Hash);
      // Console.WriteLine(firstOutPoint.N);
      // Console.WriteLine(transaction.Inputs.Count);
      OutPoint firstPreviousOutPoint = transaction.Inputs.First().PrevOut;
      var firstPreviousTransaction = client.GetTransaction(firstPreviousOutPoint.Hash).Result.Transaction;
      // Console.WriteLine(firstPreviousTransaction.IsCoinBase);

//Get spent amount
      Money spentAmount = Money.Zero;
      foreach (var spentCoin in spentCoins)
      {
        spentAmount = (Money)spentCoin.Amount.Add(spentAmount);
      }
      Console.WriteLine(spentAmount.ToDecimal(MoneyUnit.BTC));
//Get received amount
      Money receivedAmount = Money.Zero;
      foreach (var receivedCoin in receivedCoins)
      {
        receivedAmount = (Money)receivedCoin.Amount.Add(receivedAmount);
      }
      Console.WriteLine(receivedAmount.ToDecimal(MoneyUnit.BTC));

//Calculate fee
      var fee = transaction.GetFee(spentCoins.ToArray());
      Console.WriteLine(fee);

//Generate key, script, address
      // Key privateKey = new Key();
      // BitcoinSecret testPrivateKey = privateKey.GetBitcoinSecret(Network.TestNet);
      // PubKey testPublicKey = testPrivateKey.PubKey;
      // BitcoinAddress testAddress = testPublicKey.GetAddress(Network.TestNet);
      // Console.WriteLine(testPrivateKey);
      // Console.WriteLine(testAddress.ScriptPubKey);
      // Console.WriteLine(testPublicKey);
      // Console.WriteLine(testAddress);

//Test if WIF private key = ECDSA hashed private key
      // bool WifIsBitcoinSecret = testNetPrivateKey == privateKey.GetWif(Network.TestNet);
      // Console.WriteLine(WifIsBitcoinSecret);


      // Key privateKey = new Key();
      // BitcoinSecret bitcoinSecret = privateKey.GetWif(Network.Main);
      // Key samePrivateKey = bitcoinSecret.PrivateKey;
      // Console.WriteLine(samePrivateKey == privateKey);


    }
  }
}


